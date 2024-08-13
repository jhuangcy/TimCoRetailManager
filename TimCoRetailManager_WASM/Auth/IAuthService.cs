using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
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

        public AuthService(HttpClient http, AuthenticationStateProvider auth, ILocalStorageService localStorage)
        {
            _http = http;
            _auth = auth;
            _localStorage = localStorage;
        }

        public async Task<Token> Login(LoginViewModel login)
        {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", login.Email),
                new KeyValuePair<string, string>("password", login.Password),
            });

            var res = await _http.PostAsync("https://localhost:44372/token", body);
            if (!res.IsSuccessStatusCode)
                return null;

            var content = await res.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<Token>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            await _localStorage.SetItemAsync("token", token.access_token);

            (_auth as StateProvider).NotifyLogin(token.access_token);
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.access_token);
            return token;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            (_auth as StateProvider).NotifyLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
