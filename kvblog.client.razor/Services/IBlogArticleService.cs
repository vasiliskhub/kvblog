using Kvblog.Client.Razor.ViewModels;

namespace Kvblog.Client.Razor.Services
{
    public interface IBlogArticleService
    {
        public Task<BlogArticle> GetAsync(Guid blogArticleId);
        public Task<PagedResult<BlogArticle>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        public Task<PagedResult<BlogArticle>> SearchAsync(string query, int pageNumber = 1, int pageSize = 10);
        public Task AddAsync(BlogArticle blogArticle);
        public Task UpdateAsync(BlogArticle blogArticle);
        public Task DeleteAsync(Guid blogArticleId);

    }
}
