using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_API.Library.Models;

namespace TimCoRetailManager_API.Library.Services
{
    public interface IProductService
    {
        Task<List<Product>> FindAll();
        Task<Product> FindOne(int id);
    }

    public class ProductService : IProductService
    {
        public async Task<List<Product>> FindAll()
        {
            IDb db = new Db();

            return await db.LoadAsync<Product, dynamic>("dbo.sp_GetAllProducts", new { }, "TimCoRetailManager_DB");
        }

        public async Task<Product> FindOne(int id)
        {
            IDb db = new Db();

            return (await db.LoadAsync<Product, dynamic>("dbo.sp_GetProduct", new { Id = id }, "TimCoRetailManager_DB")).FirstOrDefault();
        }
    }
}
