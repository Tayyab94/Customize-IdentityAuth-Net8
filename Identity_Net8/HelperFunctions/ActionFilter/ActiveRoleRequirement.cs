using Microsoft.AspNetCore.Authorization;

namespace Identity_Net8.HelperFunctions.ActionFilter
{
    public class ActiveRoleRequirement : IAuthorizationRequirement
    {
        public bool RequiredStatus { get; }

        public ActiveRoleRequirement(bool requiredStatus)
        {
            this.RequiredStatus = requiredStatus;
        }
    }

    public class ActiveRoleHandler : AuthorizationHandler<ActiveRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveRoleRequirement requirement)
        {
            var activeClaim = context.User.Claims.FirstOrDefault(s => s.Type == "activeRole")?.Value;

            if(activeClaim is not null && bool.TryParse(activeClaim, out bool isActive))
            {
                if (isActive == requirement.RequiredStatus)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
