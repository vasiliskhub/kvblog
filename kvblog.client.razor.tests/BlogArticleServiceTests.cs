using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.ViewModels;
using NUnit.Framework;

namespace Kvblog.Client.Razor.Tests;

[TestFixture]
public class BlogArticleServiceTests
{
	private HttpClient CreateClientWithHandler(MockHandler handler)
		=> new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };

	[Test]
	public async Task GetAsync_ReturnsBlogArticle()
	{
		var article = new BlogArticle { Id = Guid.NewGuid(), Title = "title" };
		var response = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(JsonSerializer.Serialize(article), Encoding.UTF8, "application/json")
		};
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		var result = await service.GetAsync(article.Id);

		Assert.That(result?.Id, Is.EqualTo(article.Id));
		Assert.That(result?.Title, Is.EqualTo("title"));
	}

	[Test]
	public async Task GetAllAsync_ReturnsPagedResult()
	{
		var paged = new PagedResult<BlogArticle>
		{
			Items = new() { new BlogArticle { Id = Guid.NewGuid(), Title = "t" } },
			PageNumber = 1,
			PageSize = 4,
			TotalCount = 1
		};
		var response = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(JsonSerializer.Serialize(paged), Encoding.UTF8, "application/json")
		};
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		var result = await service.GetAllAsync();

		Assert.That(result.Items.Count, Is.EqualTo(1));
		Assert.That(result.PageNumber, Is.EqualTo(1));
		Assert.That(result.PageSize, Is.EqualTo(4));
		Assert.That(result.TotalCount, Is.EqualTo(1));
	}

	[Test]
	public async Task SearchAsync_ReturnsPagedResult()
	{
		var paged = new PagedResult<BlogArticle>
		{
			Items = new() { new BlogArticle { Id = Guid.NewGuid(), Title = "search" } },
			PageNumber = 1,
			PageSize = 10,
			TotalCount = 1
		};
		var response = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(JsonSerializer.Serialize(paged), Encoding.UTF8, "application/json")
		};
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		var result = await service.SearchAsync("search");

		Assert.That(result.Items.Count, Is.EqualTo(1));
		Assert.That(result.Items[0].Title, Is.EqualTo("search"));
	}

	[Test]
	public async Task AddAsync_SendsPostRequest_WithDefaultImage()
	{
		var response = new HttpResponseMessage(HttpStatusCode.OK);
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		var article = new BlogArticle { Title = "t", Description = "d", Body = "b", FeaturedImageUrl = null };

		await service.AddAsync(article);

		Assert.That(handler.LastRequest, Is.Not.Null);
		Assert.That(handler.LastRequest.Method, Is.EqualTo(HttpMethod.Post));
		Assert.That(handler.LastRequestBody, Does.Contain("defaultArticle.jpg"));
	}

	[Test]
	public async Task UpdateAsync_SendsPutRequest_AndSetsDateUpdated()
	{
		var response = new HttpResponseMessage(HttpStatusCode.OK);
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		var article = new BlogArticle { Id = Guid.NewGuid(), Title = "t", Description = "d", Body = "b", FeaturedImageUrl = null };

		await service.UpdateAsync(article);

		Assert.That(handler.LastRequest, Is.Not.Null);
		Assert.That(handler.LastRequest.Method, Is.EqualTo(HttpMethod.Put));
		Assert.That(handler.LastRequestBody, Does.Contain("defaultArticle.jpg"));
		Assert.That(article.DateUpdated, Is.Not.Null);
	}

	[Test]
	public async Task DeleteAsync_SendsDeleteRequest()
	{
		var response = new HttpResponseMessage(HttpStatusCode.OK);
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		var id = Guid.NewGuid();
		await service.DeleteAsync(id);

		Assert.That(handler.LastRequest, Is.Not.Null);
		Assert.That(handler.LastRequest.Method, Is.EqualTo(HttpMethod.Delete));
		Assert.That(handler.LastRequest.RequestUri.ToString(), Does.Contain(id.ToString()));
	}

	[Test]
	public void GetAsync_ThrowsOnError()
	{
		var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
		var handler = new MockHandler(response);
		var client = CreateClientWithHandler(handler);
		var service = new BlogArticleService(client);

		Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetAsync(Guid.NewGuid()));
	}

	private class MockHandler : HttpMessageHandler
	{
		public HttpRequestMessage? LastRequest { get; private set; }
		public string? LastRequestBody { get; private set; }
		private readonly HttpResponseMessage _response;

		public MockHandler(HttpResponseMessage response)
		{
			_response = response;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			LastRequest = request;
			if (request.Content != null)
			{
				LastRequestBody = await request.Content.ReadAsStringAsync();
			}
			return _response;
		}
	}
}