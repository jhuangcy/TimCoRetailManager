using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_API.Library.Models;

namespace TimCoRetailManager_API.Library.Services
{
    public interface IUserService
    {
        Task<User> FindOneAsync(string Id);
        Task InsertOneAsync(User user);
    }

    public class UserService : IUserService
    {
        //private readonly IConfiguration _config;
        private readonly IDb _db;

        public UserService(/*IConfiguration config,*/ IDb db)
        {
            //_config = config;
            _db = db;
        }

        public async Task<User> FindOneAsync(string Id)
        {
            //IDb db = new Db(_config);

            var p = new { Id };
            return (await _db.LoadAsync<User, dynamic>("dbo.sp_GetUser", p, "TimCoRetailManager_DB")).FirstOrDefault();
        }

        public async Task InsertOneAsync(User user)
        {
            var p = new { user.IdentityUserId, user.FirstName, user.LastName, user.Email };
            await _db.SaveAsync("dbo.sp_AddUser", p, "TimCoRetailManager_DB");
        }
    }
}
