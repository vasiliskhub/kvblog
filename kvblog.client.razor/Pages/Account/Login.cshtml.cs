// Pages/Account/Login.cshtml.cs

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;

namespace Kvblog.Client.Razor.Pages.Account
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                        .WithRedirectUri(returnUrl)
                        .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
    }
}