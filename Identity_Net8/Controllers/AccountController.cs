using Identity_Net8.Models;
using Identity_Net8.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity_Net8.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public AccountController(
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        //var claims = new List<Claim>();

                        //var roles = await _userManager.GetRolesAsync(user);
                        //foreach (var roleName in roles)
                        //{
                        //    var role = await _roleManager.FindByNameAsync(roleName);
                        //    if (role != null)
                        //    {
                        //        claims.Add(new Claim("activeRole", role.IsActive.ToString()));
                        //        claims.Add(new Claim("UserHobby", user.Hobby.ToString()));
                        //    }
                        //}

                        //var identity = new ClaimsIdentity(claims, "login");
                        //await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);


                        // Alternative way, check the line 40 in program.cs file. we have added the middleware that automatically exevcute while login. 

                        var claimPrinciple = await _signInManager.CreateUserPrincipalAsync(user);

                        //// Add aditional claim if needed 
                        //var identity = (ClaimsIdentity)claimPrinciple.Identity;
                        //identity.AddClaim(new Claim("UserHobby", user.Hobby.ToString()));


                        await _signInManager.SignInAsync(user, isPersistent: false);


                        var roleDescription = User.Claims.FirstOrDefault(c => c.Type == "activeRole")?.Value;

                        var userHobby = User.Claims.FirstOrDefault(s => s.Type == "userHobby")?.Value;
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult>ReadInfo
            ()

        {
            var roleDescription = User.Claims.FirstOrDefault(c => c.Type == "activeRole")?.Value;

            var userHobby = User.Claims.FirstOrDefault(s => s.Type == "userHobby")?.Value;

            return Ok();
        }

        [Authorize(policy: "ActiveRolePolicy")]
        [HttpGet]
        public async Task<IActionResult>Usercart()
        {
            string user = "Tayyab";

           throw new Exception();
            return Ok();
        }
    }
}
