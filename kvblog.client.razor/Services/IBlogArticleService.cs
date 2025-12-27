using Kvblog.Client.Razor.ViewModels;

namespace Kvblog.Client.Razor.Services;

public interface IBlogArticleService
{
    Task<ServiceResult<BlogArticle>> GetBySlugAsync(string slug);
    Task<ServiceResult<BlogArticle>> GetByIdAsync(Guid Id);
    Task<ServiceResult<PagedResult<BlogArticle>>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<ServiceResult<PagedResult<BlogArticle>>> SearchAsync(string query, int pageNumber = 1, int pageSize = 10);
    Task<ServiceResult> AddAsync(BlogArticle blogArticle);
    Task<ServiceResult> UpdateAsync(BlogArticle blogArticle);
    Task<ServiceResult> DeleteAsync(Guid blogArticleId);
}
