using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TimCoRetailManager_API.Library.Models;
using TimCoRetailManager_API.Library.Services;

namespace TimCoRetailManager_API.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        // GET: api/Inventory/get
        [Authorize(Roles = "Manager, Admin")]
        public async Task<List<Inventory>> Get()
        {
            IInventoryService inventoryService = new InventoryService();

            return await inventoryService.FindAll();
        }

        // GET: api/Inventory/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Inventory/post
        [Authorize(Roles = "Admin")]
        public async Task Post([FromBody]Inventory inventory)
        {
            IInventoryService inventoryService = new InventoryService();

            await inventoryService.InsertOne(inventory);
        }

        // PUT: api/Inventory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Inventory/5
        public void Delete(int id)
        {
        }
    }
}
