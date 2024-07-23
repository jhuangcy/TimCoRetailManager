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
    // WebApiConfig was edited to allow action names
    [Authorize]
    public class UsersController : ApiController
    {
        // GET: api/Users
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Users/5
        public string Get(int id)
        {
            return "value";
        }

        // GET: api/Users/GetOne
        public async Task<User> GetOne()
        {
            IUserService userService = new UserService();

            var id = RequestContext.Principal.Identity.GetUserId();
            return await userService.FindOneAsync(id);
        }

        // POST: api/Users
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
        }
    }
}
