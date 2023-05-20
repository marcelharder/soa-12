using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class AdminController : BaseApiController
    {
        private UserManager<AppUser> _manager;
        public AdminController(UserManager<AppUser> manager)
        {
            _manager = manager;
        }

        [Authorize(Policy ="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _manager.Users
            .Include(r => r.UserRoles)
            .ThenInclude(r => r.Role)
            .OrderBy(u => u.UserName)
            .Select(u => new {
                u.Id,
                UserName = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();
            return Ok(users);
        }
        
        [Authorize(Policy ="RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditUserRole(string username, [FromQuery] string roles){
              var selectedRoles = roles.Split(",").ToArray();
              var user = await _manager.FindByNameAsync(username);
              if(user == null) return NotFound("Could not find this user");
              
              var userRoles = await _manager.GetRolesAsync(user);
              var results = await _manager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
              if(!results.Succeeded) return BadRequest("Failed to add to roles");
              results = await _manager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
              if(!results.Succeeded) return BadRequest("Failed to remove from roles");
              return Ok(await _manager.GetRolesAsync(user));
        }


    }
}
