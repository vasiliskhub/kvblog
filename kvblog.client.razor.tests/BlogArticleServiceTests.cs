using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.Utilities;
using Kvblog.Client.Razor.ViewModels;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Kvblog.Client.Razor.Tests;

class MockHandler : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }
    private readonly HttpResponseMessage _response;

    public MockHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        return Task.FromResult(_response);
    }
}

[TestFixture]
public class BlogArticleServiceTests
{
    [Test]
    public async Task GetAsync_ReturnsBlogArticle()
    {
        var article = new BlogArticle { Id = Guid.NewGuid(), Title = "title" };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(article), Encoding.UTF8, "application/json")
        };
        var handler = new MockHandler(response);
        var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost") };
        var service = new BlogArticleService(client);

        var result = await service.GetAsync(article.Id);

        Assert.That(result?.Id, Is.EqualTo(article.Id));
    }

    [Test]
    public async Task AddAsync_SendsPostRequest()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var handler = new MockHandler(response);
        var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost") };
        var service = new BlogArticleService(client);
        var article = new BlogArticle { Title = "t", Description="d", Body="b" };

        await service.AddAsync(article);

        Assert.That(handler.LastRequest?.Method, Is.EqualTo(HttpMethod.Post));
    }

    [Test]
    public void BlogArticleUtils_GetReadTime_ReturnsValue()
    {
        var text = string.Join(" ", Enumerable.Repeat("word", 220));
        var html = $"<p>{text}</p>";

        var readTime = BlogArticleUtils.GetReadTime(html);

        Assert.That(readTime, Is.EqualTo(3));
    }
}
