using Kvblog.Api.Contracts.Responses;
using Kvblog.Api.Contracts.Requests;

namespace Kvblog.Api.Application.Services;

public interface IBlogService
{
    Task<Result<BlogArticleResponse>> GetArticleByIdAsync(Guid id);
    Task<Result<BlogArticleResponse>> GetArticleBySlugAsync(string slug);
    Task<Result<PagedResultResponse<BlogArticleResponse>>> GetAllArticlesAsync(int pageNumber, int pageSize);
    Task<Result<PagedResultResponse<BlogArticleResponse>>> SearchArticlesAsync(string query, int pageNumber, int pageSize);
    Task<Result<Guid>> CreateArticleAsync(BlogArticleUpsertRequest article);
    Task<Result> UpdateArticleAsync(Guid id, BlogArticleUpsertRequest article);
    Task<Result> DeleteArticleAsync(Guid id);
}
