using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kvblog.Client.Razor.Utilities
{
    public static class UserRoleHelper
    {
        public static bool IsAdmin(AuthorizationHandlerContext context)
        {
            return HasAdminRole(context.User.Claims);
        }

        public static bool IsAdmin(IEnumerable<Claim> userClaims)
        {
            return HasAdminRole(userClaims);
        }

		public static string? GetUsername(IEnumerable<Claim> userClaims)
		{
			var usernameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "nickname");
			return usernameClaim?.Value;
		}

		private static bool HasAdminRole(IEnumerable<Claim> userClaims)
        {
            // Just in case access to permissions is needed.
            //if (context.Resource is HttpContext httpContext)
            //{
            //    var token = await httpContext.GetTokenAsync("access_token");
            //    var handler = new JwtSecurityTokenHandler();
            //    var jwtSecurityToken = handler.ReadJwtToken(token);
            //}

            var roles = userClaims.FirstOrDefault(c => c.Type == "https://apokapa.eu/roles");

            if (roles is null)
            {
                return false;
            }
            if (roles.Value == "admin")
            {
                return true;
            }

            return false;
        }
    }
}
