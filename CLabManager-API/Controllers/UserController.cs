using CLabManager_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ModelsLibrary.Models.DTO;
using System.Threading.Tasks.Dataflow;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [HttpGet]
        public async Task<IEnumerable<UserWithRoleDTO>> GetUsersWithRoles()
        {
            var users = await _db.Users.ToListAsync();
            List<UserWithRoleDTO> usersWithRoles = new();
            if (users != null)
            {
                foreach (var user in users)
                {
                    usersWithRoles.Add(new UserWithRoleDTO
                    {
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Id = user.Id,
                        Role = (await _userManager.GetRolesAsync(user))[0]

                    });
                }
            }

            return usersWithRoles;
        }

        [HttpPut]
        public async Task<ActionResult<UserWithRoleDTO>> UpdateUserRole(RoleUpdateDTO data)
        {
            List<string> roles = new List<string>()
            {
                "Admin",
                "User"
            };
            var user = await _userManager.FindByIdAsync(data.UserId);
            if (user != null)
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
                var result = await _userManager.AddToRoleAsync(user, data.Role);
                if (result.Succeeded)
                {
                    return new UserWithRoleDTO
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Id = user.Id,
                        Role = data.Role
                    };
                }
            }
            return NotFound();
        }
    }
}
