using Kvblog.Api.Application.Repositories;
using Kvblog.Api.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kvblog.Api.Db.Tests;

[TestFixture]
public class BlogArticleRepositoryTests
{
	private BlogDbContext _dbContext;
	private BlogArticleRepository _repository;

	[SetUp]
	public void SetUp()
	{
		var options = new DbContextOptionsBuilder<BlogDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;
		_dbContext = new BlogDbContext(options);
		_repository = new BlogArticleRepository(_dbContext);
	}

	[TearDown]
	public void TearDown()
	{
		_dbContext.Dispose();
	}

	[Test]
	public async Task AddAsync_AddsEntityToDatabase()
	{
		var article = new BlogArticleEntity
		{
			Id = Guid.NewGuid(),
			Title = "Test Title",
			Body = "Test Body",
			Description = "Test Desc", // Required
			DatePosted = DateTime.UtcNow,
			DateUpdated = DateTime.UtcNow,
			Author = "Test Author",
			Slug = "test-title"
		};

		await _repository.AddAsync(article);

		var fromDb = await _dbContext.BlogArticles.FindAsync(article.Id);
		Assert.NotNull(fromDb);
		Assert.That(fromDb.Title, Is.EqualTo("Test Title"));
	}

	[Test]
	public async Task GetByIdAsync_ReturnsCorrectEntity()
	{
		var article = new BlogArticleEntity
		{
			Id = Guid.NewGuid(),
			Title = "Find Me",
			Body = "Body",
			Description = "desc", // Required
			DatePosted = DateTime.UtcNow,
			DateUpdated = DateTime.UtcNow,
			Author = "Test Author",
			Slug = "find-me"
		};
		_dbContext.BlogArticles.Add(article);
		await _dbContext.SaveChangesAsync();

		var result = await _repository.GetByIdAsync(article.Id);

		Assert.NotNull(result);
		Assert.That(result.Id, Is.EqualTo(article.Id));
		Assert.That(result.Title, Is.EqualTo("Find Me"));
	}

	[Test]
	public async Task GetAllAsync_ReturnsPagedResult()
	{
		for (int i = 0; i < 15; i++)
		{
			_dbContext.BlogArticles.Add(new BlogArticleEntity
			{
				Id = Guid.NewGuid(),
				Title = $"Title {i}",
				Body = "Body",
				Description = $"desc {i}", // Required
				DatePosted = DateTime.UtcNow.AddDays(-i),
				DateUpdated = DateTime.UtcNow.AddDays(-i),
				Author = $"Author {i}",
				Slug = $"title-{i}"
			});
		}
		await _dbContext.SaveChangesAsync();

		var result = await _repository.GetAllAsync(2, 5);

		Assert.That(result.Items.Count, Is.EqualTo(5));
		Assert.That(result.PageNumber, Is.EqualTo(2));
		Assert.That(result.PageSize, Is.EqualTo(5));
		Assert.That(result.TotalCount, Is.EqualTo(15));
	}

	[Test]
	public async Task UpdateAsync_UpdatesEntity()
	{
		var article = new BlogArticleEntity
		{
			Id = Guid.NewGuid(),
			Title = "Old Title",
			Body = "Old Body",
			Description = "Old Desc",
			DatePosted = DateTime.UtcNow.AddDays(-5),
			DateUpdated = DateTime.UtcNow.AddDays(-2),
			Author = "Old Author",
			Slug = "old-title"
		};
		_dbContext.BlogArticles.Add(article);
		await _dbContext.SaveChangesAsync();

		// Save original values for fields that should not change
		var originalDescription = article.Description;
		var originalDatePosted = article.DatePosted;
		var originalDateUpdated = article.DateUpdated;
		var originalAuthor = article.Author;

		// Only update Title and Body
		article.Title = "New Title";
		article.Body = "New Body";
		await _repository.UpdateAsync(article);

		var fromDb = await _dbContext.BlogArticles.FindAsync(article.Id);
		Assert.That(fromDb.Title, Is.EqualTo("New Title"));
		Assert.That(fromDb.Body, Is.EqualTo("New Body"));

		// Check that the rest of the fields stayed the same
		Assert.That(fromDb.Description, Is.EqualTo(originalDescription));
		Assert.That(fromDb.DatePosted, Is.EqualTo(originalDatePosted));
		Assert.That(fromDb.DateUpdated, Is.EqualTo(originalDateUpdated));
		Assert.That(fromDb.Author, Is.EqualTo(originalAuthor));
	}

	[Test]
	public async Task DeleteAsync_RemovesEntity()
	{
		var article = new BlogArticleEntity
		{
			Id = Guid.NewGuid(),
			Title = "To Delete",
			Body = "Body",
			Description = "desc", // Required
			DatePosted = DateTime.UtcNow,
			DateUpdated = DateTime.UtcNow,
			Author = "Test Author",
			Slug = "to-delete"
		};
		_dbContext.BlogArticles.Add(article);
		await _dbContext.SaveChangesAsync();

		await _repository.DeleteAsync(article.Id);

		var fromDb = await _dbContext.BlogArticles.FindAsync(article.Id);
		Assert.Null(fromDb);
	}

	[Test]
	public async Task GetBySlugAsync_ReturnsCorrectEntity()
	{
		var article = new BlogArticleEntity
		{
			Id = Guid.NewGuid(),
			Title = "Unique Slug Title",
			Body = "Body",
			Description = "desc",
			DatePosted = DateTime.UtcNow,
			DateUpdated = DateTime.UtcNow,
			Author = "Test Author",
			Slug = "unique-slug-title"
		};
		_dbContext.BlogArticles.Add(article);
		await _dbContext.SaveChangesAsync();

		var result = await _repository.GetBySlugAsync("unique-slug-title");

		Assert.NotNull(result);
		Assert.That(result.Id, Is.EqualTo(article.Id));
		Assert.That(result.Slug, Is.EqualTo("unique-slug-title"));
	}

	[Test]
	public async Task GetBySlugAsync_ReturnsNullIfNotFound()
	{
		var result = await _repository.GetBySlugAsync("non-existent-slug");
		Assert.IsNull(result);
	}
}