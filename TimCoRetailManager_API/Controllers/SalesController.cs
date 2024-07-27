using Microsoft.AspNet.Identity;
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
    public class SalesController : ApiController
    {
        // GET: api/Sales
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sales/getreports
        [Authorize(Roles = "Admin, Manager")]
        public async Task<List<SaleUserViewModel>> GetReports()
        {
            if (RequestContext.Principal.IsInRole("Admin"))
            { }

            ISaleService saleService = new SaleService();

            return await saleService.FindAllSalesWithUsers();
        }

        // GET: api/Sales/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sales/post
        [Authorize(Roles = "Cashier")]
        public async Task Post([FromBody]SaleDTO sale)
        {
            ISaleService saleService = new SaleService();

            var id = RequestContext.Principal.Identity.GetUserId();
            await saleService.InsertOne(sale, id);
        }

        // PUT: api/Sales/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sales/5
        public void Delete(int id)
        {
        }
    }
}
