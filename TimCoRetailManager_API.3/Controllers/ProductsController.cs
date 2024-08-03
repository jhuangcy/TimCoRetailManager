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
    [Authorize(Roles = "Cashier")]
    public class ProductsController : ControllerBase
    {
        //private readonly IConfiguration _config;
        private readonly IProductService _productService;

        public ProductsController(/*IConfiguration config,*/ IProductService productService)
        {
            //_config = config;
            _productService = productService;
        }

        // GET: api/Products/get
        [HttpGet]
        public async Task<List<Product>> Get()
        {
            //IProductService productService = new ProductService(_config);
            return await _productService.FindAll();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
