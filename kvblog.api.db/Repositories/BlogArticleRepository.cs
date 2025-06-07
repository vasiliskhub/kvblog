using Kvblog.Api.Db.Entities;
using Kvblog.Api.Db.Interfaces;
using Kvblog.Api.Db;
using Kvblog.Api.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Kvblog.Api.Db.Repositories
{
    public class BlogArticleRepository : IBlogArticleRepository
    {
        private readonly BlogDbContext _dbContext;

        public BlogArticleRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogArticleEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.BlogArticles.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedResultDto<BlogArticleEntity>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.BlogArticles.AsQueryable().OrderByDescending(a => a.DatePosted);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<BlogArticleEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

		//ToDo : Implement full-text search with PostgreSQL, Materialized tsvector Column
		public async Task<PagedResultDto<BlogArticleEntity>> SearchAsync(string query, int pageNumber, int pageSize)
        {
            var resultQuery = _dbContext.BlogArticles
                .Where(b => EF.Functions.ToTsVector("english", b.Title + " " + b.Description)
                    .Matches(EF.Functions.PlainToTsQuery("english", query)))
                .OrderByDescending(b => EF.Functions.ToTsVector("english", b.Title + " " + b.Description)
                    .Rank(EF.Functions.PlainToTsQuery("english", query)));

            var totalCount = await resultQuery.CountAsync();
            var items = await resultQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<BlogArticleEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task AddAsync(BlogArticleEntity entity)
        {
            await _dbContext.BlogArticles.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(BlogArticleEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbContext.BlogArticles.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
