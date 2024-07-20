﻿using System;
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
    }

    public class UserService : IUserService
    {
        public async Task<User> FindOneAsync(string Id)
        {
            IDb db = new Db();

            var p = new { Id };
            return (await db.LoadAsync<User, dynamic>("dbo.sp_GetUser", p, "TimCoRetailManager_DB")).FirstOrDefault();
        }
    }
}
