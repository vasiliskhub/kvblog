// Pages/Account/Login.cshtml.cs

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Kvblog.Client.Razor.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/")
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}