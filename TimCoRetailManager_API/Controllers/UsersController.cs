using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TimCoRetailManager_API.Library.Models;
using TimCoRetailManager_API.Library.Services;
using TimCoRetailManager_API.Models;

// WebApiConfig was edited to allow action names
namespace TimCoRetailManager_API.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        // GET: api/Users/get
        [Authorize(Roles = "Admin")]
        public List<UserDTO> Get()
        {
            var userDtos = new List<UserDTO>();

            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    var userDto = new UserDTO() { Id = user.Id, Email = user.Email };
                    foreach (var role in user.Roles)
                    {
                        userDto.Roles.Add(role.RoleId, roles.Find(r => r.Id == role.RoleId).Name);
                    }
                    userDtos.Add(userDto);
                }
            }

            return userDtos;
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
