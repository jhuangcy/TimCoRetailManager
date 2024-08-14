using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

// This is to extend the functionality of the original with our own custom stuff
namespace TimCoRetailManager_WASM.Auth
{
    public class StateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        readonly AuthenticationState anonymous;

        public StateProvider(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("token");
            if (string.IsNullOrWhiteSpace(token))
                return anonymous;

            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtHelper.ParseClaims(token), "jwtAuthType")));
        }

        public void NotifyLogin(string token)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(JwtHelper.ParseClaims(token), "jwtAuthType"));
            var state = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(state);
        }

        public void NotifyLogout()
        {
            var state = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(state);
        }
    }
}
