using AutoMapper;
using Kvblog.Api.Db.Entities;
using Kvblog.Api.Db.Interfaces;
using Kvblog.Api.Interfaces;
using Kvblog.Api.Models;

namespace Kvblog.Api.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogArticleRepository _blogArticleRepository;
        private readonly IMapper _mapper;

        public BlogService(IBlogArticleRepository blogRepository, IMapper mapper)
        {
            _blogArticleRepository = blogRepository;
            _mapper = mapper;
        }
        
        public async Task<BlogArticle> GetArticleByIdAsync(Guid id)
        {
            var articleEntity = await _blogArticleRepository.GetByIdAsync(id);
            var article = _mapper.Map<BlogArticle>(articleEntity);
            return article;
        }

        public async Task<PagedResult<BlogArticle>> GetAllArticlesAsync(int pageNumber, int pageSize)
        {
            var pagedDto = await _blogArticleRepository.GetAllAsync(pageNumber, pageSize);
            var articles = _mapper.Map<List<BlogArticle>>(pagedDto.Items);

            return new PagedResult<BlogArticle>
            {
                Items = articles,
                PageNumber = pagedDto.PageNumber,
                PageSize = pagedDto.PageSize,
                TotalCount = pagedDto.TotalCount
            };
        }

        public async Task<PagedResult<BlogArticle>> SearchArticlesAsync(string query, int pageNumber, int pageSize)
        {
            var pagedDto = await _blogArticleRepository.SearchAsync(query, pageNumber, pageSize);
            var articles = _mapper.Map<List<BlogArticle>>(pagedDto.Items);

            return new PagedResult<BlogArticle>
            {
                Items = articles,
                PageNumber = pagedDto.PageNumber,
                PageSize = pagedDto.PageSize,
                TotalCount = pagedDto.TotalCount
            };
        }

        public async Task CreateArticleAsync(BlogArticleUpsert article)
        {
            var articleEntity = _mapper.Map<BlogArticleEntity>(article);
            await _blogArticleRepository.AddAsync(articleEntity);
        }

        public async Task UpdateArticleAsync(Guid id, BlogArticleUpsert article)
        {
            var existingEntity = await _blogArticleRepository.GetByIdAsync(id);

            if (existingEntity == null)
            {
                return;
            }

			existingEntity.DateUpdated ??= DateTime.UtcNow;
			existingEntity.Title = article.Title;
            existingEntity.Description = article.Description;   
            existingEntity.Body = article.Body;

			await _blogArticleRepository.UpdateAsync(existingEntity);
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            await _blogArticleRepository.DeleteAsync(id);
        }
    }
}
