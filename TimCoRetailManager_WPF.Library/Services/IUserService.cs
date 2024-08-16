using System;
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
    public interface IUserService
    {
        Task<Token> GetTokenAsync(string username, string password);
        Task GetUserAsync(string token);
        Task<List<UserDTO>> GetAllAsync();
        Task<Dictionary<string, string>> GetAllRolesAsync();
        Task AddRoleToUser(string userId, string role);
        Task RemoveRoleFromUser(string userId, string role);
        Task PostAsync(RegisterViewModel register);
    }

    public class UserService : IUserService
    {
        private readonly IApi _api;
        private readonly IUser _user;

        public UserService(IApi api, IUser user)
        {
            _api = api;
            _user = user;
        }

        public async Task<Token> GetTokenAsync(string username, string password)
        {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (var res = await _api.Http.PostAsync("/token", body))
            {
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<Token>();

                throw new Exception(res.ReasonPhrase);
            }
        }

        public async Task GetUserAsync(string token)
        {
            // https://stackoverflow.com/questions/14627399/setting-authorization-header-of-httpclient
            _api.Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            using (var res = await _api.Http.GetAsync("/api/users/getone"))
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

        public async Task<List<UserDTO>> GetAllAsync()
        {
            using (var res = await _api.Http.GetAsync("/api/users/get"))
            {
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<List<UserDTO>>();
                else
                    throw new Exception(res.ReasonPhrase);
            }
        }

        public async Task<Dictionary<string, string>> GetAllRolesAsync()
        {
            using (var res = await _api.Http.GetAsync("/api/users/getroles"))
            {
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<Dictionary<string, string>>();
                else
                    throw new Exception(res.ReasonPhrase);
            }
        }

        public async Task AddRoleToUser(string userId, string role)
        {
            using (var res = await _api.Http.PostAsJsonAsync("/api/users/addrole", new { userId, role }))
            {
                if (!res.IsSuccessStatusCode)
                    throw new Exception(res.ReasonPhrase);
            }
        }

        public async Task RemoveRoleFromUser(string userId, string role)
        {
            using (var res = await _api.Http.PostAsJsonAsync("/api/users/removerole", new { userId, role }))
            {
                if (!res.IsSuccessStatusCode)
                    throw new Exception(res.ReasonPhrase);
            }
        }

        public async Task PostAsync(RegisterViewModel register)
        {
            var body = new { register.FirstName, register.LastName, register.Email, register.Password };

            using (var res = await _api.Http.PostAsJsonAsync("/api/users/register", body))
            {
                if (!res.IsSuccessStatusCode)
                    throw new Exception(res.ReasonPhrase);
            }
        }
    }
}
