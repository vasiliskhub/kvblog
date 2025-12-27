using Kvblog.Client.Razor.ViewModels;
using System.Text;
using System.Text.Json;

namespace Kvblog.Client.Razor.Services;

public class BlogArticleService : IBlogArticleService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BlogArticleService> _logger;
    private const string BlogArticlesBaseApiUrl = "api/v1/blogarticles";

    public BlogArticleService(HttpClient httpClient, ILogger<BlogArticleService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ServiceResult> AddAsync(BlogArticle blogArticle)
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
            var response = await _httpClient.PostAsync(apiUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to AddAsync: {errorMessage}");
                return ServiceResult.Failure(errorMessage);
            }

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to AddAsync: {ex.Message}");
            return ServiceResult.Failure("An unexpected error occurred while creating the article");
        }
    }

    public async Task<ServiceResult<PagedResult<BlogArticle>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var apiUrl = $"{BlogArticlesBaseApiUrl}?pageNumber={pageNumber}&pageSize={pageSize}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to GetAllAsync: {errorMessage}");
                return ServiceResult<PagedResult<BlogArticle>>.Failure(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<PagedResult<BlogArticle>>();
            return ServiceResult<PagedResult<BlogArticle>>.Success(result ?? new PagedResult<BlogArticle>());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to GetAllAsync: {ex.Message}");
            return ServiceResult<PagedResult<BlogArticle>>.Failure("An unexpected error occurred while fetching articles");
        }
    }

    public async Task<ServiceResult<PagedResult<BlogArticle>>> SearchAsync(string query, int pageNumber = 1, int pageSize = 10)
    {
        var apiUrl = $"{BlogArticlesBaseApiUrl}/search?query={Uri.EscapeDataString(query)}&pageNumber={pageNumber}&pageSize={pageSize}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to SearchAsync: {errorMessage}");
                return ServiceResult<PagedResult<BlogArticle>>.Failure(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<PagedResult<BlogArticle>>();
            return ServiceResult<PagedResult<BlogArticle>>.Success(result ?? new PagedResult<BlogArticle>());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to SearchAsync: {ex.Message}");
            return ServiceResult<PagedResult<BlogArticle>>.Failure("An unexpected error occurred while searching articles");
        }
    }

    public async Task<ServiceResult<BlogArticle>> GetBySlugAsync(string Slug)
    {
        var apiUrl = $"{BlogArticlesBaseApiUrl}/{Slug}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to GetBySlugAsync: {errorMessage}");
                return ServiceResult<BlogArticle>.Failure(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<BlogArticle>();
            return ServiceResult<BlogArticle>.Success(result!);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to GetBySlugAsync: {ex.Message}");
            return ServiceResult<BlogArticle>.Failure("An unexpected error occurred while fetching the article");
        }
    }

    public async Task<ServiceResult<BlogArticle>> GetByIdAsync(Guid Id)
    {
        var apiUrl = $"{BlogArticlesBaseApiUrl}/{Id}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to GetByIdAsync: {errorMessage}");
                return ServiceResult<BlogArticle>.Failure(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<BlogArticle>();
            return ServiceResult<BlogArticle>.Success(result!);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to GetByIdAsync: {ex.Message}");
            return ServiceResult<BlogArticle>.Failure("An unexpected error occurred while fetching the article");
        }
    }

    public async Task<ServiceResult> UpdateAsync(BlogArticle blogArticle)
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
            var response = await _httpClient.PutAsync(apiUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to UpdateAsync: {errorMessage}");
                return ServiceResult.Failure(errorMessage);
            }

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to UpdateAsync: {ex.Message}");
            return ServiceResult.Failure("An unexpected error occurred while updating the article");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid blogArticleId)
    {
        var apiUrl = $"{BlogArticlesBaseApiUrl}/{blogArticleId}";

        try
        {
            var response = await _httpClient.DeleteAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await TryParseProblemDetails(response);
                _logger.LogError($"Failed to DeleteAsync: {errorMessage}");
                return ServiceResult.Failure(errorMessage);
            }

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to DeleteAsync: {ex.Message}");
            return ServiceResult.Failure("An unexpected error occurred while deleting the article");
        }
    }

    private async Task<string> TryParseProblemDetails(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);

            if (doc.RootElement.TryGetProperty("detail", out var detailProp))
            {
                return detailProp.GetString() ?? $"Request failed with status {response.StatusCode}";
            }

            if (doc.RootElement.TryGetProperty("title", out var titleProp))
            {
                return titleProp.GetString() ?? $"Request failed with status {response.StatusCode}";
            }
        }
        catch
        {
            // If parsing fails, return generic message
        }

        return $"Request failed with status {response.StatusCode}";
    }
}
