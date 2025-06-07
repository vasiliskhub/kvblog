using Kvblog.Client.Razor.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Client.Razor.Pages.Components.ArticleList
{
    public class ArticleListViewComponent : ViewComponent
    {
        private IBlogArticleService _blogArticleService;

        public ArticleListViewComponent(IBlogArticleService blogArticleService)
        {
            _blogArticleService = blogArticleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _blogArticleService.GetAllAsync();
            return View(result.Items);
        }
    }
}
