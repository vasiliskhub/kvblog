using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.Utilities;
using Kvblog.Client.Razor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kvblog.Client.Razor.Pages.Public;

public class BlogArticleModel : PageModel
{
    private readonly IBlogArticleService _blogArticleService;

    public BlogArticleModel(IBlogArticleService blogArticleService)
    {
        _blogArticleService = blogArticleService;
    }

    [FromRoute]
    public string Slug { get; set; } = string.Empty;

    [BindProperty]
    public BlogArticle BlogArticle { get; set; } = new();

    public async Task OnGet()
    {
        var result = await _blogArticleService.GetBySlugAsync(Slug);

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

    public int GetReadTime(string? htmlContent)
    {
        return BlogArticleUtils.GetReadTime(htmlContent);
    }

    public string GetPostedDate(DateTime? dateTime)
    {
        return BlogArticleUtils.GetPostedDate(dateTime);
    }
}
