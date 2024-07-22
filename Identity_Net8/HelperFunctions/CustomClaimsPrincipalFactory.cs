using Identity_Net8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;

namespace Identity_Net8.HelperFunctions
{
    public class CustomClaimsPrincipalFactory :UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CustomClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,

            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {// Assuming IsActive is a nullable boolean (bool?)
                    bool isActive = role.IsActive ==true? true:false;
                    identity.AddClaim(new Claim("activeRole", isActive.ToString()));

                    // you can add more claim as you want
                    identity.AddClaim(new Claim("userHobby", user.Hobby));
                }
            }

            return identity;
        }
    }
}
