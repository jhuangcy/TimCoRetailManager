using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TimCoRetailManager_WASM.Models;

namespace TimCoRetailManager_WASM.Auth
{
    public interface IAuthService
    {
        Task<Token> Login(LoginViewModel login);
        Task Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _auth;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;

        string tokenStorage;

        public AuthService(HttpClient http, AuthenticationStateProvider auth, ILocalStorageService localStorage, IConfiguration config)
        {
            _http = http;
            _auth = auth;
            _localStorage = localStorage;
            _config = config;

            tokenStorage = config["tokenStorage"];
        }

        public async Task<Token> Login(LoginViewModel login)
        {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", login.Email),
                new KeyValuePair<string, string>("password", login.Password),
            });

            var res = await _http.PostAsync(_config["api"] + _config["tokenEndpoint"], body);
            if (!res.IsSuccessStatusCode)
                return null;

            var content = await res.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<Token>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            await _localStorage.SetItemAsync(tokenStorage, token.access_token);

            await (_auth as StateProvider).NotifyLoginAsync(token.access_token);
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.access_token);
            return token;
        }

        public async Task Logout() => await (_auth as StateProvider).NotifyLogoutAsync();
    }
}
