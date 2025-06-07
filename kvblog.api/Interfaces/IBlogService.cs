using Kvblog.Api.Models;

namespace Kvblog.Api.Interfaces
{
	public interface IBlogService
	{
                Task<BlogArticle> GetArticleByIdAsync(Guid id);
        Task<PagedResult<BlogArticle>> GetAllArticlesAsync(int pageNumber, int pageSize);
        Task<PagedResult<BlogArticle>> SearchArticlesAsync(string query, int pageNumber, int pageSize);
        Task CreateArticleAsync(BlogArticleUpsert article);
		Task UpdateArticleAsync(Guid id, BlogArticleUpsert article);
		Task DeleteArticleAsync(Guid id);
	}
}
