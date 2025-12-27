using Kvblog.Api.Application.Services;
using Kvblog.Api.Contracts.Requests;
using Kvblog.Api.Contracts.Responses;
using Kvblog.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Api.Controllers;

[ApiController]
public class BlogArticleController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogArticleController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet(ApiEndpoints.BlogArticles.GetById)]
    public async Task<ActionResult<BlogArticleResponse>> GetArticleByIdAsync([FromRoute] Guid id)
    {
        return (await _blogService.GetArticleByIdAsync(id)).ToActionResult();
    }

    [HttpGet(ApiEndpoints.BlogArticles.GetBySlug)]
    public async Task<ActionResult<BlogArticleResponse>> GetArticleBySlugAsync([FromRoute] string slug)
    {
        return (await _blogService.GetArticleBySlugAsync(slug)).ToActionResult();
    }

    [HttpGet(ApiEndpoints.BlogArticles.GetAll)]
    public async Task<ActionResult<PagedResultResponse<BlogArticleResponse>>> GetAllArticlesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return (await _blogService.GetAllArticlesAsync(pageNumber, pageSize)).ToActionResult();
    }

    [HttpGet(ApiEndpoints.BlogArticles.Search)]
    public async Task<ActionResult<PagedResultResponse<BlogArticleResponse>>> SearchArticlesAsync([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Search query cannot be empty"
            });
        }

        if (query.Length > 200)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Search query too long"
            });
        }

        return (await _blogService.SearchArticlesAsync(query, pageNumber, pageSize)).ToActionResult();
    }

    [HttpPost(ApiEndpoints.BlogArticles.Create)]
    [Authorize(Policy = "CUDAccess")]
    public async Task<IActionResult> CreateArticleAsync([FromBody] BlogArticleUpsertRequest article)
    {
        return (await _blogService.CreateArticleAsync(article))
            .ToCreatedResult(Request, "/api/v1/blogarticles");
    }

    [HttpPut(ApiEndpoints.BlogArticles.Update)]
    [Authorize(Policy = "CUDAccess")]
    public async Task<IActionResult> UpdateArticleAsync([FromRoute] Guid id, [FromBody] BlogArticleUpsertRequest article)
    {
        return (await _blogService.UpdateArticleAsync(id, article)).ToActionResult();
    }

    [HttpDelete(ApiEndpoints.BlogArticles.Delete)]
    [Authorize(Policy = "CUDAccess")]
    public async Task<IActionResult> DeleteArticleAsync([FromRoute] Guid id)
    {
        return (await _blogService.DeleteArticleAsync(id)).ToActionResult();
    }
}
