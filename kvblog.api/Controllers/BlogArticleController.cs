using Kvblog.Api.Application.Services;
using Kvblog.Api.Contracts.Requests;
using Kvblog.Api.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Api.Controllers
{
    [ApiController]
    public class BlogArticleController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogArticleController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet(ApiEndpoints.BlogArticles.GetById)]
        public async Task<ActionResult<BlogArticleResponse>> GetArticleById([FromRoute] Guid id)
        {
            var article = await _blogService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

		[HttpGet(ApiEndpoints.BlogArticles.GetBySlug)]
		public async Task<ActionResult<BlogArticleResponse>> GetArticleById([FromRoute] string slug)
		{
			var article = await _blogService.GetArticleBySlugAsync(slug);

			if (article == null)
			{
				return NotFound();
			}

			return Ok(article);
		}

		[HttpGet(ApiEndpoints.BlogArticles.GetAll)]
        public async Task<ActionResult<PagedResultResponse<BlogArticleResponse>>> GetAllArticles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var articles = await _blogService.GetAllArticlesAsync(pageNumber, pageSize);

            if (articles == null || articles.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(articles);
        }

        [HttpGet(ApiEndpoints.BlogArticles.Search)]
        public async Task<ActionResult<PagedResultResponse<BlogArticleResponse>>> SearchArticles([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var articles = await _blogService.SearchArticlesAsync(query, pageNumber, pageSize);

            return Ok(articles);
        }

		[HttpPost(ApiEndpoints.BlogArticles.Create)]
        [Authorize(Policy = "CUDAccess")]
        public async Task<IActionResult> CreateArticle([FromBody] BlogArticleUpsertRequest article)
        {
            await _blogService.CreateArticleAsync(article);

			//ToDo : Return the createdataction article's ID or the full article object
			return Created();
        }

        [HttpPut(ApiEndpoints.BlogArticles.Update)]
        [Authorize(Policy = "CUDAccess")]
        public async Task<IActionResult> UpdateArticle([FromRoute] Guid id, [FromBody] BlogArticleUpsertRequest article)
        {
            await _blogService.UpdateArticleAsync(id, article);

            return NoContent();
        }

        [HttpDelete(ApiEndpoints.BlogArticles.Delete)]
        [Authorize(Policy = "CUDAccess")]
        public async Task<IActionResult> DeleteArticle([FromRoute] Guid id)
        {
            var existingArticle = await _blogService.GetArticleByIdAsync(id);

            if (existingArticle == null)
            {
                return NotFound();
            }

            await _blogService.DeleteArticleAsync(id);

            return NoContent();
        }
    }
}
