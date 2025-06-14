using Kvblog.Client.Razor.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Client.Razor.Pages.Components.ArticleList
{
    public class AdminArticleListViewComponent : ViewComponent
    {
        private IBlogArticleService _blogArticleService;

        public AdminArticleListViewComponent(IBlogArticleService blogArticleService)
        {
            _blogArticleService = blogArticleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _blogArticleService.GetAllAsync(1, int.MaxValue);
            return View(result.Items);
        }
    }
}
