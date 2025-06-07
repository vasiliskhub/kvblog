using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kvblog.Client.Razor.Pages
{
    public class AboutThisModel : PageModel
    {
        private readonly ILogger<AboutThisModel> _logger;

        public AboutThisModel(ILogger<AboutThisModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}