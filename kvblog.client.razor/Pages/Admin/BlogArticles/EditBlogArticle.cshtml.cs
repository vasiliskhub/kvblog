using Kvblog.Client.Razor.Services;
using Microsoft.AspNetCore.Mvc;
using Kvblog.Client.Razor.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kvblog.Client.Razor.Pages.Admin.BlogArticles;

public class EditBlogArticleModel : PageModel
{

    private readonly IBlogArticleService _blogArticleService;

    public EditBlogArticleModel(IBlogArticleService blogArticleService)
    {
        _blogArticleService = blogArticleService;
    }

    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public BlogArticle BlogArticle { get; set; } = new();

    public async Task OnGet()
    {
        var result = await _blogArticleService.GetByIdAsync(Id);

        if (result.IsSuccess)
        {
            BlogArticle = result.Value!;
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            BlogArticle = new BlogArticle();
        }
    }

    public async Task<IActionResult> OnPostEdit()
    {
        if (!ModelState.IsValid)
        { return Page(); }

        var result = await _blogArticleService.UpdateAsync(BlogArticle);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Blog article updated successfully";
            return RedirectToPage("/Admin/Dashboard");
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDelete()
    {
        var result = await _blogArticleService.DeleteAsync(Id);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Blog article deleted successfully";
            return RedirectToPage("/Admin/Dashboard");
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
