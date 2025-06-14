using Kvblog.Api.Contracts.Responses;
using Kvblog.Api.Contracts.Requests;

namespace Kvblog.Api.Application.Services
{
	public interface IBlogService
	{
        Task<BlogArticleResponse> GetArticleByIdAsync(Guid id);
		Task<BlogArticleResponse> GetArticleBySlugAsync(string slug);
		Task<PagedResultResponse<BlogArticleResponse>> GetAllArticlesAsync(int pageNumber, int pageSize);
        Task<PagedResultResponse<BlogArticleResponse>> SearchArticlesAsync(string query, int pageNumber, int pageSize);
        Task CreateArticleAsync(BlogArticleUpsertRequest article);
		Task UpdateArticleAsync(Guid id, BlogArticleUpsertRequest article);
		Task DeleteArticleAsync(Guid id);
	}
}
