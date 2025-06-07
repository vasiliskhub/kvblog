using Kvblog.Client.Razor.Utilities.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;

namespace Kvblog.Client.Razor.Utilities.AuthPolicies
{
    public class IsAdminHandler : AuthorizationHandler<IsAdminRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            if (UserRoleHelper.IsAdmin(context))
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}
