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
    public class ProductsController : ApiController
    {
        // GET: api/Products/get
        public async Task<List<Product>> Get()
        {
            IProductService productService = new ProductService();
            return await productService.FindAll();
        }

        // GET: api/Products/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Products
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Products/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
        }
    }
}
