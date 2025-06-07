using AutoMapper;
using Kvblog.Api.Db.Entities;
using Kvblog.Api.Db.Interfaces;
using Kvblog.Api.Interfaces;
using Kvblog.Api.Models;
using Kvblog.Api.Services;
using Kvblog.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Kvblog.Api.Tests;

public class BlogServiceTests
{
    [Test]
    public async Task GetArticleByIdAsync_ReturnsMappedArticle()
    {
        var id = Guid.NewGuid();
        var entity = new BlogArticleEntity { Id = id, Title = "t" };
        var expected = new BlogArticle { Id = id, Title = "t" };

        var repo = Substitute.For<IBlogArticleRepository>();
        repo.GetByIdAsync(id).Returns(entity);
        var mapper = Substitute.For<IMapper>();
        mapper.Map<BlogArticle>(entity).Returns(expected);

        var service = new BlogService(repo, mapper);
        var result = await service.GetArticleByIdAsync(id);

        Assert.That(result?.Id, Is.EqualTo(id));
        Assert.That(result?.Title, Is.EqualTo("t"));
    }

    [Test]
    public async Task CreateArticleAsync_CallsRepository()
    {
        var upsert = new BlogArticleUpsert { Title = "t", Body = "b" };
        var entity = new BlogArticleEntity { Id = Guid.NewGuid() };
        var repo = Substitute.For<IBlogArticleRepository>();
        var mapper = Substitute.For<IMapper>();
        mapper.Map<BlogArticleEntity>(upsert).Returns(entity);

        var service = new BlogService(repo, mapper);
        await service.CreateArticleAsync(upsert);

        await repo.Received(1).AddAsync(entity);
    }

    [Test]
    public async Task UpdateArticleAsync_PreservesExistingValues()
    {
        var id = Guid.NewGuid();
        var existing = new BlogArticleEntity
        {
            Id = id,
            Title = "old",
            Body = "body",
            Description = "desc",
            FeaturedImageUrl = "img",
            DatePosted = DateTime.UtcNow.AddDays(-1),
            Author = "author",
            DateUpdated = DateTime.UtcNow.AddDays(-1)
        };

        var repo = Substitute.For<IBlogArticleRepository>();
        repo.GetByIdAsync(id).Returns(existing);
        BlogArticleEntity? saved = null;
        repo.UpdateAsync(Arg.Do<BlogArticleEntity>(e => saved = e)).Returns(Task.CompletedTask);

        var mapper = Substitute.For<IMapper>();
        mapper.Map<BlogArticleEntity>(Arg.Any<BlogArticleUpsert>()).Returns(ci =>
        {
            var up = ci.Arg<BlogArticleUpsert>();
            return new BlogArticleEntity
            {
                Title = up.Title,
                Description = up.Description,
                Body = up.Body,
                FeaturedImageUrl = up.FeaturedImageUrl,
                DatePosted = up.DatePosted,
                DateUpdated = up.DateUpdated,
                Author = up.Author
            };
        });

        var service = new BlogService(repo, mapper);
        var upsert = new BlogArticleUpsert
        {
            Title = "new",
            Description = "desc2",
            Body = "body2",
            FeaturedImageUrl = "img2",
            DatePosted = null,
            Author = null,
            DateUpdated = DateTime.UtcNow
        };

        await service.UpdateArticleAsync(id, upsert);

        Assert.NotNull(saved);
        Assert.That(saved!.DatePosted, Is.EqualTo(existing.DatePosted));
        Assert.That(saved.Author, Is.EqualTo(existing.Author));
    }
}

public class BlogArticleControllerTests
{
    [Test]
    public async Task GetArticleById_ReturnsNotFound_WhenArticleMissing()
    {
        var service = Substitute.For<IBlogService>();
        service.GetArticleByIdAsync(Arg.Any<Guid>()).Returns((BlogArticle?)null);
        var controller = new BlogArticleController(service);

        var result = await controller.GetArticleById(Guid.NewGuid());

        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public async Task GetArticleById_ReturnsOk_WithArticle()
    {
        var article = new BlogArticle { Id = Guid.NewGuid(), Title = "a" };
        var service = Substitute.For<IBlogService>();
        service.GetArticleByIdAsync(article.Id).Returns(article);
        var controller = new BlogArticleController(service);

        var result = await controller.GetArticleById(article.Id);

        var ok = result.Result as OkObjectResult;
        Assert.NotNull(ok);
        var returned = ok!.Value as BlogArticle;
        Assert.NotNull(returned);
        Assert.That(returned!.Id, Is.EqualTo(article.Id));
    }
}
