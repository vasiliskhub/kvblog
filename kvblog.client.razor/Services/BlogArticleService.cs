using Kvblog.Client.Razor.ViewModels;
using System.Text;
using System.Text.Json;

namespace Kvblog.Client.Razor.Services
{
    public class BlogArticleService : IBlogArticleService
    {
        private readonly HttpClient _httpClient;

        public BlogArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddAsync(BlogArticle blogArticle)
        {
            var apiUrl = "BlogArticle";

            blogArticle.FeaturedImageUrl ??= "defaultArticle.jpg";

            // Serialize the object to JSON
            using StringContent jsonContent = new(
                                                    JsonSerializer.Serialize(new
                                                    {
                                                        blogArticle.Title,
                                                        blogArticle.Description,
                                                        blogArticle.Body,
                                                        blogArticle.FeaturedImageUrl,
                                                        blogArticle.DatePosted,
                                                        blogArticle.DateUpdated,
                                                        blogArticle.Author
                                                    }),
                                                    Encoding.UTF8,
                                                    "application/json");

            // Send an HTTP POST request to the API endpoint
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, jsonContent);
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                // You can log the exception or handle it in another way
                Console.WriteLine("Error fetching data: " + ex.Message);
                throw;
            }
        }

        public async Task<PagedResult<BlogArticle>> GetAllAsync(int pageNumber = 1, int pageSize = 4)
        {
            var apiUrl = $"BlogArticle?pageNumber={pageNumber}&pageSize={pageSize}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<PagedResult<BlogArticle>>(apiUrl);
                return response ?? new PagedResult<BlogArticle>();
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                // You can log the exception or handle it in another way
                Console.WriteLine("Error fetching data: " + ex.Message);

                // Return empty result in case of an exception
                return new PagedResult<BlogArticle>();
            }
        }

        public async Task<PagedResult<BlogArticle>> SearchAsync(string query, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"BlogArticle/search?query={Uri.EscapeDataString(query)}&pageNumber={pageNumber}&pageSize={pageSize}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<PagedResult<BlogArticle>>(apiUrl);
                return response ?? new PagedResult<BlogArticle>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching data: " + ex.Message);
                return new PagedResult<BlogArticle>();
            }
        }

        public async Task<BlogArticle> GetAsync(Guid blogArticleId)
        {
            var apiUrl = $"BlogArticle/{blogArticleId}";

            try
            {
                var blogArticlesResponse = await _httpClient.GetFromJsonAsync<BlogArticle>(apiUrl);
                return blogArticlesResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                // You can log the exception or handle it in another way
                Console.WriteLine("Error fetching data: " + ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(BlogArticle blogArticle)
        {
            var apiUrl = $"BlogArticle/{blogArticle.Id}";

            blogArticle.FeaturedImageUrl ??= "defaultArticle.jpg";
            blogArticle.DateUpdated = DateTime.UtcNow;

            // Serialize the object to JSON
            using StringContent jsonContent = new(
                                                    JsonSerializer.Serialize(new
                                                    {
                                                        blogArticle.Title,
                                                        blogArticle.Description,
                                                        blogArticle.Body,
                                                        blogArticle.FeaturedImageUrl,
                                                        blogArticle.DatePosted,
                                                        blogArticle.DateUpdated,
                                                        blogArticle.Author
                                                    }),
                                                    Encoding.UTF8,
                                                    "application/json");

            // Send an HTTP PUT request to the API endpoint
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsync(apiUrl, jsonContent);
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                // You can log the exception or handle it in another way
                Console.WriteLine("Error fetching data: " + ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(Guid blogArticleId)
        {
            var apiUrl = $"BlogArticle/{blogArticleId}";

            // Send an HTTP DELETE request to the API endpoint
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                // You can log the exception or handle it in another way
                Console.WriteLine("Error fetching data: " + ex.Message);
                throw;
            }
        }
    }
}
