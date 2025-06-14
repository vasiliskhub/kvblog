using Kvblog.Client.Razor.ViewModels;
using System.Text;
using System.Text.Json;

namespace Kvblog.Client.Razor.Services
{
	public class BlogArticleService : IBlogArticleService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<BlogArticleService> _logger;
		private const string BlogArticlesBaseApiUrl = "api/v1/blogarticles";

		public BlogArticleService(HttpClient httpClient, ILogger<BlogArticleService> logger)
		{
			_httpClient = httpClient;
			_logger= logger;
		}

		public async Task AddAsync(BlogArticle blogArticle)
		{
			var apiUrl = BlogArticlesBaseApiUrl;

			using StringContent jsonContent = new(
													JsonSerializer.Serialize(new
													{
														blogArticle.Title,
														blogArticle.Description,
														blogArticle.Body,
														blogArticle.DatePosted,
														blogArticle.DateUpdated,
														blogArticle.Author
													}),
													Encoding.UTF8,
													"application/json");

			try
			{
				HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, jsonContent);
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to AddAsync:" + ex.Message);
				throw;
			}
		}

		public async Task<PagedResult<BlogArticle>> GetAllAsync(int pageNumber, int pageSize)
		{
			var apiUrl = $"{BlogArticlesBaseApiUrl}?pageNumber={pageNumber}&pageSize={pageSize}";

			try
			{
				var response = await _httpClient.GetFromJsonAsync<PagedResult<BlogArticle>>(apiUrl);
				return response ?? new PagedResult<BlogArticle>();
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to GetAllAsync:" + ex.Message);
				throw;
			}
		}

		public async Task<PagedResult<BlogArticle>> SearchAsync(string query, int pageNumber = 1, int pageSize = 10)
		{
			var apiUrl = $"{BlogArticlesBaseApiUrl}/search?query={Uri.EscapeDataString(query)}&pageNumber={pageNumber}&pageSize={pageSize}";

			try
			{
				var response = await _httpClient.GetFromJsonAsync<PagedResult<BlogArticle>>(apiUrl);
				return response ?? new PagedResult<BlogArticle>();
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to SearchAsync:" + ex.Message);
				throw;
			}
		}

		public async Task<BlogArticle> GetBySlugAsync(string Slug)
		{
			var apiUrl = $"{BlogArticlesBaseApiUrl}/{Slug}";

			try
			{
				var blogArticlesResponse = await _httpClient.GetFromJsonAsync<BlogArticle>(apiUrl);
				return blogArticlesResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to GetBySlugAsync: " + ex.Message);
				throw;
			}
		}

		public async Task<BlogArticle> GetByIdAsync(Guid Id)
		{
			var apiUrl = $"{BlogArticlesBaseApiUrl}/{Id}";

			try
			{
				var blogArticlesResponse = await _httpClient.GetFromJsonAsync<BlogArticle>(apiUrl);
				return blogArticlesResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to GetByIdAsync: " + ex.Message);
				throw;
			}
		}

		public async Task UpdateAsync(BlogArticle blogArticle)
		{
			var apiUrl = $"{BlogArticlesBaseApiUrl}/{blogArticle.Id}";

			blogArticle.DateUpdated = DateTime.UtcNow;

			using StringContent jsonContent = new(
													JsonSerializer.Serialize(new
													{
														blogArticle.Title,
														blogArticle.Description,
														blogArticle.Body,
														blogArticle.DatePosted,
														blogArticle.DateUpdated,
														blogArticle.Author
													}),
													Encoding.UTF8,
													"application/json");

			try
			{
				HttpResponseMessage response = await _httpClient.PutAsync(apiUrl, jsonContent);
				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					_logger.LogError($"Failed to update blog article: {errorContent}");
				}

			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to update blog article: " + ex.Message);
				throw;
			}
		}

		public async Task DeleteAsync(Guid blogArticleId)
		{
			var apiUrl = $"{BlogArticlesBaseApiUrl}/{blogArticleId}";

			// Send an HTTP DELETE request to the API endpoint
			try
			{
				HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);
			}
			catch (Exception ex)
			{
				_logger.LogError("Error deleting data: " + ex.Message);
				throw;
			}
		}
	}
}
