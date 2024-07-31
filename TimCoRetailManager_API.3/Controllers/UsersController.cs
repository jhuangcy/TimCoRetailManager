using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimCoRetailManager_API._3.Data;
using TimCoRetailManager_API.Library.Models;
using TimCoRetailManager_API.Library.Services;
using TimCoRetailManager_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimCoRetailManager_API._3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users/get
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<UserDTO> Get()
        {
            var userDtos = new List<UserDTO>();

            //var userStore = new UserStore<ApplicationUser>(_context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            //var users = userManager.Users.ToList();
            var users = _context.Users.ToList();
            //var roles = _context.Roles.ToList();
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles
                        on ur.RoleId equals r.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                var userDto = new UserDTO() { Id = user.Id, Email = user.Email };

                //foreach (var role in user.Roles)
                //{
                //    userDto.Roles.Add(role.RoleId, roles.Find(r => r.Id == role.RoleId).Name);
                //}
                userDto.Roles = roles.Where(r => r.UserId == userDto.Id).ToDictionary(r => r.RoleId, r => r.Name);

                userDtos.Add(userDto);
            }
            

            return userDtos;
        }

        // GET: api/Users/getroles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public Dictionary<string, string> GetRoles() => _context.Roles.ToDictionary(r => r.Id, r => r.Name);

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // GET: api/Users/GetOne
        [HttpGet]
        public async Task<User> GetOne()
        {
            IUserService userService = new UserService();

            //var id = RequestContext.Principal.Identity.GetUserId();
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await userService.FindOneAsync(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST: api/Users/addrole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task AddRole(UserRole userRole)
        {
            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            var user = await _userManager.FindByIdAsync(userRole.UserId);
            //await _userManager.AddToRoleAsync(userRole.UserId, userRole.Role);
            await _userManager.AddToRoleAsync(user, userRole.Role);
            
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // DELETE: api/Users/removerole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task RemoveRole(UserRole userRole)
        {
            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            var user = await _userManager.FindByIdAsync(userRole.UserId);
            //await _userManager.RemoveFromRoleAsync(userRole.UserId, userRole.Role);
            await _userManager.RemoveFromRoleAsync(user, userRole.Role);
        }
    }
}
