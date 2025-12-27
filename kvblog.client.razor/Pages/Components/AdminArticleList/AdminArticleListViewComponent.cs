using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Client.Razor.Pages.Components.ArticleList;

public class AdminArticleListViewComponent : ViewComponent
{
    private readonly IBlogArticleService _blogArticleService;

    public AdminArticleListViewComponent(IBlogArticleService blogArticleService)
    {
        _blogArticleService = blogArticleService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await _blogArticleService.GetAllAsync(1, int.MaxValue);

        if (result.IsSuccess)
        {
            return View(result.Value!.Items);
        }

        return View(new List<BlogArticle>());
    }
}
