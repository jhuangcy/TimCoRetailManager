using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Models;

namespace TimCoRetailManager_WPF.Services
{
    public interface IApiService
    {
        Task<Token> GetTokenAsync(string username, string password);
    }

    public class ApiService : IApiService
    {
        HttpClient http { get; set; }

        public ApiService()
        {
            InitHttp();
        }

        void InitHttp()
        {
            var api = ConfigurationManager.AppSettings["api"];

            http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.BaseAddress = new Uri(api);
        }

        public async Task<Token> GetTokenAsync(string username, string password)
        {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (var res = await http.PostAsync("/token", body))
            {
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<Token>();

                throw new Exception(res.ReasonPhrase);
            }
        }
    }
}
