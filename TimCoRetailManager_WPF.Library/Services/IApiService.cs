﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library.Models;

namespace TimCoRetailManager_WPF.Library.Services
{
    public interface IApiService
    {
        HttpClient Http { get; }

        Task<Token> GetTokenAsync(string username, string password);
        Task GetUserAsync(string token);
    }

    public class ApiService : IApiService
    {
        HttpClient _http;
        public HttpClient Http => _http;

        private readonly IUser _user;

        public ApiService(IUser user)
        {
            InitHttp();
            _user = user;
        }

        void InitHttp()
        {
            var api = ConfigurationManager.AppSettings["api"];

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http.BaseAddress = new Uri(api);
        }

        public async Task<Token> GetTokenAsync(string username, string password)
        {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (var res = await _http.PostAsync("/token", body))
            {
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<Token>();

                throw new Exception(res.ReasonPhrase);
            }
        }

        public async Task GetUserAsync(string token)
        {
            // https://stackoverflow.com/questions/14627399/setting-authorization-header-of-httpclient
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            using (var res = await _http.GetAsync("/api/users/getone"))
            {
                if (res.IsSuccessStatusCode)
                {
                    var user = await res.Content.ReadAsAsync<User>();

                    _user.IdentityUserId = user.IdentityUserId;
                    _user.FirstName = user.FirstName;
                    _user.LastName = user.LastName;
                    _user.Email = user.Email;
                    _user.CreatedDate = user.CreatedDate;
                    _user.Token = token;
                }
                else 
                    throw new Exception(res.ReasonPhrase);
            }
        }
    }
}
