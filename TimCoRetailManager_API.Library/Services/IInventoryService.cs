using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_API.Library.Models;

namespace TimCoRetailManager_API.Library.Services
{
    public interface IInventoryService
    {
        Task<List<Inventory>> FindAll();
        Task InsertOne(Inventory inventory);
    }

    public class InventoryService : IInventoryService
    {
        public async Task<List<Inventory>> FindAll()
        {
            IDb db = new Db();

            return await db.LoadAsync<Inventory, dynamic>("dbo.sp_GetAllInventory", new { }, "TimCoRetailManager_DB");
        }

        public async Task InsertOne(Inventory inventory)
        {
            IDb db = new Db();

            await db.SaveAsync("dbo.sp_AddInventory", inventory, "TimCoRetailManager_DB");
        }
    }
}
