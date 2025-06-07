using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.Utilities;
using Kvblog.Client.Razor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kvblog.Client.Razor.Pages.Admin.BlogArticles
{
    public class AddBlogArticleModel : PageModel
    {
        private IBlogArticleService _blogArticleService;

        public AddBlogArticleModel(IBlogArticleService blogArticleService,
           IWebHostEnvironment environment)
        {
            _blogArticleService = blogArticleService;
        }

        [BindProperty]
        public BlogArticle BlogArticle { get; set; }

        public void OnGet()
        {
            BlogArticle = new BlogArticle();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) { return Page(); }

            BlogArticle.DatePosted = DateTime.UtcNow;
            BlogArticle.DateUpdated = DateTime.UtcNow;
            BlogArticle.Author = UserRoleHelper.GetUsername(User.Claims);

            await _blogArticleService.AddAsync(BlogArticle);

            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
