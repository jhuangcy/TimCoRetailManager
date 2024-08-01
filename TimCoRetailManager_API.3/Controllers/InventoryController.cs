using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimCoRetailManager_API.Library.Models;
using TimCoRetailManager_API.Library.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimCoRetailManager_API._3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InventoryController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Inventory/get
        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<List<Inventory>> Get()
        {
            IInventoryService inventoryService = new InventoryService(_config);

            return await inventoryService.FindAll();
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Inventory/post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task Post([FromBody] Inventory inventory)
        {
            IInventoryService inventoryService = new InventoryService(_config);

            await inventoryService.InsertOne(inventory);
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
