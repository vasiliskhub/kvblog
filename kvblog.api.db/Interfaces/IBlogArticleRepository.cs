using Kvblog.Api.Db.Entities;
using Kvblog.Api.Db.Models;

namespace Kvblog.Api.Db.Interfaces
{
    public interface IBlogArticleRepository
    {
        Task<BlogArticleEntity> GetByIdAsync(Guid id);
        Task<PagedResultDto<BlogArticleEntity>> GetAllAsync(int pageNumber, int pageSize);
        Task<PagedResultDto<BlogArticleEntity>> SearchAsync(string query, int pageNumber, int pageSize);
        Task AddAsync(BlogArticleEntity entity);
        Task UpdateAsync(BlogArticleEntity entity);
        Task DeleteAsync(Guid id);
    }
}
