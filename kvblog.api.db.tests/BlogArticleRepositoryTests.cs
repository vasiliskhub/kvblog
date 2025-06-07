using Kvblog.Api.Db.Entities;
using Kvblog.Api.Db.Repositories;
using Kvblog.Api.Db;
using Microsoft.EntityFrameworkCore;

namespace Kvblog.Api.Db.Tests;

[TestFixture]
public class BlogArticleRepositoryTests
{
    private DbContextOptions<BlogDbContext> _options = null!;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Test]
    public async Task AddAsync_AddsEntityToContext()
    {
        var entity = new BlogArticleEntity
        {
            Id = Guid.NewGuid(),
            Title = "t",
            Description = "d",
            Body = "b",
            FeaturedImageUrl = "img"
        };
        await using var context = new BlogDbContext(_options);
        var repo = new BlogArticleRepository(context);

        await repo.AddAsync(entity);

        var saved = await context.BlogArticles.FindAsync(entity.Id);
        Assert.NotNull(saved);
    }

    [Test]
    public async Task GetAllAsync_ReturnsPagedResult()
    {
        await using var context = new BlogDbContext(_options);
        context.BlogArticles.AddRange(
            new BlogArticleEntity { Id = Guid.NewGuid(), Title = "t1", Body="b", Description="d", FeaturedImageUrl="img" },
            new BlogArticleEntity { Id = Guid.NewGuid(), Title = "t2", Body="b", Description="d", FeaturedImageUrl="img" }
        );
        await context.SaveChangesAsync();
        var repo = new BlogArticleRepository(context);

        var result = await repo.GetAllAsync(1, 1);

        Assert.That(result.TotalCount, Is.EqualTo(2));
        Assert.That(result.Items.Count, Is.EqualTo(1));
        Assert.That(result.PageNumber, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteAsync_RemovesEntity()
    {
        var entity = new BlogArticleEntity { Id = Guid.NewGuid(), Title="t", Body="b", Description="d", FeaturedImageUrl="img" };
        await using var context = new BlogDbContext(_options);
        context.BlogArticles.Add(entity);
        await context.SaveChangesAsync();
        var repo = new BlogArticleRepository(context);

        await repo.DeleteAsync(entity.Id);

        var deleted = await context.BlogArticles.FindAsync(entity.Id);
        Assert.IsNull(deleted);
    }
}
