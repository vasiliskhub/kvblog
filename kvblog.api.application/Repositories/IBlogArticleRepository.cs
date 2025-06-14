using Kvblog.Api.Application.Dtos;
using Kvblog.Api.Db.Entities;

namespace Kvblog.Api.Application.Repositories
{
    public interface IBlogArticleRepository
    {
        Task<BlogArticleEntity> GetByIdAsync(Guid id);
		Task<BlogArticleEntity> GetBySlugAsync(string slug);
		Task<PagedResultDto<BlogArticleEntity>> GetAllAsync(int pageNumber, int pageSize);
        Task<PagedResultDto<BlogArticleEntity>> SearchAsync(string query, int pageNumber, int pageSize);
        Task AddAsync(BlogArticleEntity entity);
        Task UpdateAsync(BlogArticleEntity entity);
        Task DeleteAsync(Guid id);
    }
}
