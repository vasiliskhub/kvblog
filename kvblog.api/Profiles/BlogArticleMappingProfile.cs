using AutoMapper;
using Kvblog.Api.Db.Entities;
using Kvblog.Api.Models;

public class BlogArticleMappingProfile : Profile
{
    public BlogArticleMappingProfile()
    {
        CreateMap<BlogArticleEntity, BlogArticle>();
        CreateMap<BlogArticle, BlogArticleEntity>();
        CreateMap<BlogArticleEntity, BlogArticleUpsert>();
        CreateMap<BlogArticleUpsert, BlogArticleEntity>();
    }
}