using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_API.Library
{
    internal interface IDb
    {
        Task<List<T>> LoadAsync<T, U>(string sp, U p, string name);
        Task SaveAsync<U>(string sp, U p, string name);
    }

    class Db : IDb
    {
        string GetConString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public async Task<List<T>> LoadAsync<T, U>(string sp, U p, string name)
        {
            string conString = GetConString(name);
            using (var con = new SqlConnection(conString))
            {
                return (await con.QueryAsync<T>(sp, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task SaveAsync<U>(string sp, U p, string name)
        {
            string conString = GetConString(name);
            using (var con = new SqlConnection(conString))
            {
                await con.ExecuteAsync(sp, p, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
