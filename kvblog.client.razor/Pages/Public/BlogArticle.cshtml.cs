using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.Utilities;
using Kvblog.Client.Razor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kvblog.Client.Razor.Pages.Public
{
    public class BlogArticleModel : PageModel
    {
		private IBlogArticleService _blogArticleService;

		public BlogArticleModel(IBlogArticleService blogArticleService)
		{
			_blogArticleService = blogArticleService;
		}

		[FromRoute]
		public string Slug { get; set; }

		[BindProperty]
		public BlogArticle BlogArticle { get; set; }

		public async Task OnGet()
        {
			BlogArticle = await _blogArticleService.GetBySlugAsync(Slug);
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
}
