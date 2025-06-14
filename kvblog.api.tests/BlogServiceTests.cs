using AutoMapper;
using Kvblog.Api.Application.Dtos;
using Kvblog.Api.Application.Mapping;
using Kvblog.Api.Application.Repositories;
using Kvblog.Api.Application.Services;
using Kvblog.Api.Contracts.Requests;
using Kvblog.Api.Db.Entities;
using NSubstitute;

namespace Kvblog.Api.Tests;

public class BlogServiceTests
{
	private IMapper _mapper;

	[SetUp]
	public void Setup()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<BlogArticleMappingProfile>();
		});
		_mapper = config.CreateMapper();
	}

	[Test]
	public async Task GetArticleByIdAsync_ReturnsMappedArticle()
	{
		var id = Guid.NewGuid();
		var entity = new BlogArticleEntity { Id = id, Title = "t", Body = "b", Description = "d" };
		var repo = Substitute.For<IBlogArticleRepository>();
		repo.GetByIdAsync(id).Returns(entity);

		var service = new BlogService(repo, _mapper);
		var result = await service.GetArticleByIdAsync(id);

		Assert.That(result?.Id, Is.EqualTo(id));
		Assert.That(result?.Title, Is.EqualTo("t"));
		Assert.That(result?.Body, Is.EqualTo("b"));
		Assert.That(result?.Description, Is.EqualTo("d"));
	}

	[Test]
	public async Task GetAllArticlesAsync_ReturnsPagedResult()
	{
		var id = Guid.NewGuid();
		var entity = new BlogArticleEntity { Id = id, Title = "t", Body = "b", Description = "d" };
		var pagedDto = new PagedResultDto<BlogArticleEntity>
		{
			Items = new List<BlogArticleEntity> { entity },
			PageNumber = 2,
			PageSize = 5,
			TotalCount = 1
		};
		var repo = Substitute.For<IBlogArticleRepository>();
		repo.GetAllAsync(2, 5).Returns(pagedDto);

		var service = new BlogService(repo, _mapper);
		var result = await service.GetAllArticlesAsync(2, 5);

		Assert.That(result.Items.Count, Is.EqualTo(1));
		Assert.That(result.Items[0].Id, Is.EqualTo(id));
		Assert.That(result.PageNumber, Is.EqualTo(2));
		Assert.That(result.PageSize, Is.EqualTo(5));
		Assert.That(result.TotalCount, Is.EqualTo(1));
	}

	[Test]
	public async Task SearchArticlesAsync_ReturnsPagedResult()
	{
		var id = Guid.NewGuid();
		var entity = new BlogArticleEntity { Id = id, Title = "search", Body = "b", Description = "d" };
		var pagedDto = new PagedResultDto<BlogArticleEntity>
		{
			Items = new List<BlogArticleEntity> { entity },
			PageNumber = 1,
			PageSize = 10,
			TotalCount = 1
		};
		var repo = Substitute.For<IBlogArticleRepository>();
		repo.SearchAsync("search", 1, 10).Returns(pagedDto);

		var service = new BlogService(repo, _mapper);
		var result = await service.SearchArticlesAsync("search", 1, 10);

		Assert.That(result.Items.Count, Is.EqualTo(1));
		Assert.That(result.Items[0].Title, Is.EqualTo("search"));
		Assert.That(result.PageNumber, Is.EqualTo(1));
		Assert.That(result.PageSize, Is.EqualTo(10));
		Assert.That(result.TotalCount, Is.EqualTo(1));
	}

	[Test]
	public async Task CreateArticleAsync_CallsRepositoryWithMappedEntity()
	{
		var upsert = new BlogArticleUpsertRequest { Title = "t", Body = "b", Description = "d" };
		var repo = Substitute.For<IBlogArticleRepository>();

		var service = new BlogService(repo, _mapper);
		await service.CreateArticleAsync(upsert);

		await repo.Received(1).AddAsync(Arg.Is<BlogArticleEntity>(e =>
			e.Title == "t" && e.Body == "b" && e.Description == "d"));
	}

	[Test]
	public async Task UpdateArticleAsync_UpdatesExistingEntity()
	{
		var id = Guid.NewGuid();
		var existing = new BlogArticleEntity
		{
			Id = id,
			Title = "old",
			Body = "oldbody",
			Description = "olddesc",
			DateUpdated = null
		};
		var repo = Substitute.For<IBlogArticleRepository>();
		repo.GetByIdAsync(id).Returns(existing);

		var upsert = new BlogArticleUpsertRequest
		{
			Title = "new",
			Body = "newbody",
			Description = "newdesc"
		};

		var service = new BlogService(repo, _mapper);
		await service.UpdateArticleAsync(id, upsert);

		Assert.That(existing.Title, Is.EqualTo("new"));
		Assert.That(existing.Body, Is.EqualTo("newbody"));
		Assert.That(existing.Description, Is.EqualTo("newdesc"));
		Assert.That(existing.DateUpdated, Is.Not.Null);
		await repo.Received(1).UpdateAsync(existing);
	}

	[Test]
	public async Task UpdateArticleAsync_DoesNothingIfEntityNotFound()
	{
		var id = Guid.NewGuid();
		var repo = Substitute.For<IBlogArticleRepository>();
		repo.GetByIdAsync(id).Returns((BlogArticleEntity)null);

		var upsert = new BlogArticleUpsertRequest { Title = "t", Body = "b", Description = "d" };
		var service = new BlogService(repo, _mapper);

		await service.UpdateArticleAsync(id, upsert);

		await repo.DidNotReceive().UpdateAsync(Arg.Any<BlogArticleEntity>());
	}

	[Test]
	public async Task DeleteArticleAsync_CallsRepository()
	{
		var id = Guid.NewGuid();
		var repo = Substitute.For<IBlogArticleRepository>();
		var service = new BlogService(repo, _mapper);

		await service.DeleteArticleAsync(id);

		await repo.Received(1).DeleteAsync(id);
	}
}