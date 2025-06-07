using Kvblog.Client.Razor.Services;
using Microsoft.AspNetCore.Mvc;
using Kvblog.Client.Razor.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kvblog.Client.Razor.Pages.Admin.BlogArticles
{
    public class EditBlogArticleModel : PageModel
    {

        private IBlogArticleService _blogArticleService;

        public EditBlogArticleModel(IBlogArticleService blogArticleService)
        {
            _blogArticleService = blogArticleService;
        }

        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public BlogArticle BlogArticle { get; set; }

        public async Task OnGet()
        {
            BlogArticle = await _blogArticleService.GetAsync(Id);
        }

        public async Task<IActionResult> OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            await _blogArticleService.UpdateAsync(BlogArticle);

            return RedirectToPage("/Admin/Dashboard");
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await _blogArticleService.DeleteAsync(Id);

            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
