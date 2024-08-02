using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_API._3.Data;

namespace TimCoRetailManager_API._3.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/token")]
        public async Task<IActionResult> Create(string username, string password, string grant_type)
        {
            if (await IsValidUser(username, password))
                return new ObjectResult(await GenerateToken(username));
            
            return BadRequest();
        }

        async Task<bool> IsValidUser(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _context.UserRoles join r in _context.Roles
                        on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),    // make token valid immediately
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),    // expires
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name));

            // https://stackoverflow.com/questions/52153459/what-should-be-the-key-length-in-signingcredentials-jwt-asp-net-core
            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkeyneedstobelongerthan16chars")), 
                        SecurityAlgorithms.HmacSha256
                    )
                ), 
                new JwtPayload(claims)
            );

            return new { access_token = new JwtSecurityTokenHandler().WriteToken(token), username };
        }
    }
}
