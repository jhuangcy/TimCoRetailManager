using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library;
using TimCoRetailManager_WPF.Library.Services;

// This is to extend the functionality of the original with our own custom stuff
namespace TimCoRetailManager_WASM.Auth
{
    public class StateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private readonly IApi _api;
        private readonly IUserService _userService;

        readonly AuthenticationState anonymous;

        public StateProvider(HttpClient http, ILocalStorageService localStorage, IConfiguration config, IApi api, IUserService userService)
        {
            _http = http;
            _localStorage = localStorage;
            _config = config;
            _api = api;
            _userService = userService;

            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(_config["tokenStorage"]);
            if (string.IsNullOrWhiteSpace(token))
                return anonymous;

            var success = await NotifyLoginAsync(token);
            if (!success)
                return anonymous;

            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtHelper.ParseClaims(token), "jwtAuthType")));
        }

        public async Task<bool> NotifyLoginAsync(string token)
        {
            // To keep user logged in if no internet
            try
            {
                await _userService.GetUserAsync(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                var user = new ClaimsPrincipal(new ClaimsIdentity(JwtHelper.ParseClaims(token), "jwtAuthType"));
                var state = Task.FromResult(new AuthenticationState(user));
                NotifyAuthenticationStateChanged(state);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await NotifyLogoutAsync();
                return false;
            }
        }

        public async Task NotifyLogoutAsync()
        {
            _api.ClearHeaders();
            _http.DefaultRequestHeaders.Authorization = null;
            await _localStorage.RemoveItemAsync(_config["tokenStorage"]);
            var state = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(state);
        }
    }
}
