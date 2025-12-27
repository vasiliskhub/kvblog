using Kvblog.Client.Razor.Utilities;
using Kvblog.Client.Razor.ViewModels;
using Kvblog.Client.Razor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Kvblog.Client.Razor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IBlogArticleService _blogArticleService;

    public List<BlogArticle> BlogArticles { get; set; } = new();
    public PagedResult<BlogArticle> PagedArticles { get; set; } = new();
    public IndexModel(ILogger<IndexModel> logger, IBlogArticleService blogArticleService)
    {
        _logger = logger;
        _blogArticleService = blogArticleService;
    }

    [BindProperty(SupportsGet = true, Name = "search")]
    public string? SearchQuery { get; set; }

    public async Task OnGetAsync(int pageNumber = 1)
    {
        ServiceResult<PagedResult<BlogArticle>> result;

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            result = await _blogArticleService.SearchAsync(SearchQuery, pageNumber, 4);
        }
        else
        {
            result = await _blogArticleService.GetAllAsync(pageNumber, 4);
        }

        if (result.IsSuccess)
        {
            PagedArticles = result.Value!;
            BlogArticles = PagedArticles.Items;
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            PagedArticles = new PagedResult<BlogArticle>();
            BlogArticles = new List<BlogArticle>();
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

    public string GetPreviewImage(string? htmlContent)
    {
        var src = BlogArticleUtils.GetFirstImageSrc(htmlContent);
        return string.IsNullOrWhiteSpace(src)
                ? "/themes/devblog/assets/images/blog/blog-post-thumb-2.jpg"
                : src;
    }
}
