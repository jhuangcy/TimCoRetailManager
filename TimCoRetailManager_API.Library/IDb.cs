using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_API.Library
{
    public interface IDb : IDisposable
    {
        Task<List<T>> LoadAsync<T, U>(string sp, U p, string name);
        Task SaveAsync<U>(string sp, U p, string name);

        void StartTransact(string name);
        void Commit();
        void Rollback();
        Task<List<T>> LoadTransactAsync<T, U>(string sp, U p);
        Task SaveTransactAsync<U>(string sp, U p);
    }

    public class Db : IDb, IDisposable
    {
        IDbConnection _con;
        IDbTransaction _trx;
        bool closed = false;
        private readonly IConfiguration _config;
        private readonly ILogger<IDb> _logger;

        public Db(IConfiguration config, ILogger<IDb> logger)
        {
            _config = config;
            _logger = logger;
        }

        string GetConString(string name)
        {
            //return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            return _config.GetConnectionString(name);
        }

        public async Task<List<T>> LoadAsync<T, U>(string sp, U p, string name)
        {
            string conString = GetConString(name);
            using (var con = new SqlConnection(conString))
            {
                return (await con.QueryAsync<T>(sp, p, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task SaveAsync<U>(string sp, U p, string name)
        {
            string conString = GetConString(name);
            using (var con = new SqlConnection(conString))
            {
                await con.ExecuteAsync(sp, p, commandType: CommandType.StoredProcedure);
            }
        }

        // Transaction methods
        public void StartTransact(string name)
        {
            string conString = GetConString(name);
            _con = new SqlConnection(conString);
            _con.Open();
            _trx = _con.BeginTransaction();
            closed = false;
        }

        public void Commit()
        {
            _trx?.Commit();
            _con?.Close();
            closed = true;
        }

        public void Rollback()
        {
            _trx?.Rollback();
            _con?.Close();
            closed = true;
        }

        public async Task<List<T>> LoadTransactAsync<T, U>(string sp, U p) => (await _con.QueryAsync<T>(sp, p, _trx, commandType: CommandType.StoredProcedure)).ToList();
        public async Task SaveTransactAsync<U>(string sp, U p) => await _con.ExecuteAsync(sp, p, _trx, commandType: CommandType.StoredProcedure);

        public void Dispose()
        {
            if (!closed)
            {
                try
                {
                    Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Commit transaction failed in the dispose method");
                }
            }

            _trx = null;
            _con = null;
        }
    }
}
