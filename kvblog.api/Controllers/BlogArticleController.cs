using Kvblog.Api.Interfaces;
using Kvblog.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogArticleController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogArticleController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogArticle>> GetArticleById(Guid id)
        {
            var article = await _blogService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BlogArticle>>> GetAllArticles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var articles = await _blogService.GetAllArticlesAsync(pageNumber, pageSize);

            if (articles == null || articles.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(articles);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<BlogArticle>>> SearchArticles([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var articles = await _blogService.SearchArticlesAsync(query, pageNumber, pageSize);
            return Ok(articles);
        }

		[HttpPost]
        [Authorize(Policy = "CUDAccess")]
        public async Task<IActionResult> CreateArticle(BlogArticleUpsert article)
        {
            await _blogService.CreateArticleAsync(article);

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CUDAccess")]
        public async Task<IActionResult> UpdateArticle(Guid id, BlogArticleUpsert article)
        {
            await _blogService.UpdateArticleAsync(id, article);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CUDAccess")]
        public async Task<IActionResult> DeleteArticle(Guid id)
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
