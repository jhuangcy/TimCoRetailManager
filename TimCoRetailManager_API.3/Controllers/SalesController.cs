using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimCoRetailManager_API.Library.Models;
using TimCoRetailManager_API.Library.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimCoRetailManager_API._3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        // GET: api/<SalesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sales/getreports
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<List<SaleUserViewModel>> GetReports()
        {
            //if (RequestContext.Principal.IsInRole("Admin")) { }
            if (User.IsInRole("Admin")) { }

            ISaleService saleService = new SaleService();

            return await saleService.FindAllSalesWithUsers();
        }

        // GET api/<SalesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sales/post
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public async Task Post([FromBody] SaleDTO sale)
        {
            ISaleService saleService = new SaleService();

            //var id = RequestContext.Principal.Identity.GetUserId();
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await saleService.InsertOne(sale, id);
        }

        // PUT api/<SalesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SalesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
