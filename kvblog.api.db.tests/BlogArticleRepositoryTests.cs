using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kvblog.Api.Db;
using Kvblog.Api.Db.Entities;
using Kvblog.Api.Db.Models;
using Kvblog.Api.Db.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

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
			FeaturedImageUrl = "test.jpg", // Required
			DatePosted = DateTime.UtcNow
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
			FeaturedImageUrl = "img.jpg" // Required
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
				FeaturedImageUrl = $"img{i}.jpg", // Required
				DatePosted = DateTime.UtcNow.AddDays(-i)
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
			FeaturedImageUrl = "oldimg.jpg",
			DatePosted = DateTime.UtcNow.AddDays(-5),
			DateUpdated = DateTime.UtcNow.AddDays(-2),
			Author = "Old Author"
		};
		_dbContext.BlogArticles.Add(article);
		await _dbContext.SaveChangesAsync();

		// Save original values for fields that should not change
		var originalDescription = article.Description;
		var originalFeaturedImageUrl = article.FeaturedImageUrl;
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
		Assert.That(fromDb.FeaturedImageUrl, Is.EqualTo(originalFeaturedImageUrl));
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
			FeaturedImageUrl = "img.jpg" // Required
		};
		_dbContext.BlogArticles.Add(article);
		await _dbContext.SaveChangesAsync();

		await _repository.DeleteAsync(article.Id);

		var fromDb = await _dbContext.BlogArticles.FindAsync(article.Id);
		Assert.Null(fromDb);
	}
}