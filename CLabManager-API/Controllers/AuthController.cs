using CLabManager_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ModelsLibrary.Models;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManger;
        private readonly JwtService _jwtService;
        private RoleManager<IdentityRole> _roleManager { get; }

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManger, RoleManager<IdentityRole> roleManager, JwtService jwtService)
        {
            _userManager = userManager;
            _signInManger = signInManger;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }
        [HttpPost("signin")]
        public async Task<ActionResult<LoginResponse>> SignIn(SignInUser userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(userData.Email);
            if(user == null)
            {
                return NotFound();
            }
            var token = _jwtService.CreateToken(user);
            return new LoginResponse
            {
                authResponse = token,
                data = new
                {
                    email = user.Email,
                    name = user.UserName
                }
            };
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthenticationResponse>> SignUp(SignUpUser userData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = userData.UserName,
                Email = userData.Email
            },userData.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var user = await _userManager.FindByEmailAsync(userData.Email);
            var token = _jwtService.CreateToken(user);
            return token;
        }

    }
}
