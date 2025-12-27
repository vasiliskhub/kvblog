using AutoMapper;
using Kvblog.Api.Application.Dtos;
using Kvblog.Api.Application.Repositories;
using Kvblog.Api.Contracts.Requests;
using Kvblog.Api.Contracts.Responses;
using Kvblog.Api.Db.Entities;
using System.Text.RegularExpressions;

namespace Kvblog.Api.Application.Services;

public class BlogService : IBlogService
{
    private readonly IBlogArticleRepository _blogArticleRepository;
    private readonly IMapper _mapper;

    public BlogService(IBlogArticleRepository blogRepository, IMapper mapper)
    {
        _blogArticleRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<Result<BlogArticleResponse>> GetArticleByIdAsync(Guid id)
    {
        var articleEntity = await _blogArticleRepository.GetByIdAsync(id);

        if (articleEntity == null)
        {
            return Result<BlogArticleResponse>.NotFound($"Blog article with id {id} not found");
        }

        var article = _mapper.Map<BlogArticleResponse>(articleEntity);
        return Result<BlogArticleResponse>.Success(article);
    }

    public async Task<Result<PagedResultResponse<BlogArticleResponse>>> GetAllArticlesAsync(int pageNumber, int pageSize)
    {
        var pagedDto = await _blogArticleRepository.GetAllAsync(pageNumber, pageSize);
        var articles = _mapper.Map<List<BlogArticleResponse>>(pagedDto.Items);

        var response = new PagedResultResponse<BlogArticleResponse>
        {
            Items = articles,
            PageNumber = pagedDto.PageNumber,
            PageSize = pagedDto.PageSize,
            TotalCount = pagedDto.TotalCount
        };

        return Result<PagedResultResponse<BlogArticleResponse>>.Success(response);
    }

    public async Task<Result<PagedResultResponse<BlogArticleResponse>>> SearchArticlesAsync(string query, int pageNumber, int pageSize)
    {
        var pagedDto = await _blogArticleRepository.SearchAsync(query, pageNumber, pageSize);
        var articles = _mapper.Map<List<BlogArticleResponse>>(pagedDto.Items);

        var response = new PagedResultResponse<BlogArticleResponse>
        {
            Items = articles,
            PageNumber = pagedDto.PageNumber,
            PageSize = pagedDto.PageSize,
            TotalCount = pagedDto.TotalCount
        };

        return Result<PagedResultResponse<BlogArticleResponse>>.Success(response);
    }

    public async Task<Result<Guid>> CreateArticleAsync(BlogArticleUpsertRequest article)
    {
        var articleEntity = _mapper.Map<BlogArticleEntity>(article);
        articleEntity.Id = Guid.NewGuid();
        articleEntity.DatePosted ??= DateTime.UtcNow;
        articleEntity.DateUpdated ??= DateTime.UtcNow;
        articleEntity.Slug = GenerateSlug(article.Title);
        await _blogArticleRepository.AddAsync(articleEntity);

        return Result<Guid>.Success(articleEntity.Id);
    }

    public async Task<Result> UpdateArticleAsync(Guid id, BlogArticleUpsertRequest article)
    {
        var existingEntity = await _blogArticleRepository.GetByIdAsync(id);

        if (existingEntity == null)
        {
            return Result.NotFound($"Blog article with id {id} not found");
        }

        existingEntity.DatePosted ??= DateTime.UtcNow;
        existingEntity.DateUpdated = DateTime.UtcNow;
        existingEntity.Title = article.Title;
        existingEntity.Description = article.Description;
        existingEntity.Body = article.Body;
        existingEntity.Slug = GenerateSlug(article.Title);

        await _blogArticleRepository.UpdateAsync(existingEntity);

        return Result.Success();
    }

    public async Task<Result> DeleteArticleAsync(Guid id)
    {
        var existingEntity = await _blogArticleRepository.GetByIdAsync(id);

        if (existingEntity == null)
        {
            return Result.NotFound($"Blog article with id {id} not found");
        }

        await _blogArticleRepository.DeleteAsync(id);

        return Result.Success();
    }

    public async Task<Result<BlogArticleResponse>> GetArticleBySlugAsync(string slug)
    {
        var articleEntity = await _blogArticleRepository.GetBySlugAsync(slug);

        if (articleEntity == null)
        {
            return Result<BlogArticleResponse>.NotFound($"Blog article with slug '{slug}' not found");
        }

        var article = _mapper.Map<BlogArticleResponse>(articleEntity);
        return Result<BlogArticleResponse>.Success(article);
    }

    private static string GenerateSlug(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return string.Empty;
        }

        var slug = title.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); // Remove invalid chars
        slug = Regex.Replace(slug, @"[\s_]+", "-");      // Replace spaces/underscores with hyphens
        slug = Regex.Replace(slug, @"-+", "-");          // Collapse multiple hyphens
        slug = slug.Trim('-');                           // Trim leading/trailing hyphens
        return slug;
    }
}
