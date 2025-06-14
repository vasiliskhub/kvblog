using AutoMapper;
using Kvblog.Api.Db.Entities;
using Kvblog.Api.Contracts.Responses;
using Kvblog.Api.Contracts.Requests;

namespace Kvblog.Api.Application.Mapping
{
    public class BlogArticleMappingProfile : Profile
    {
        public BlogArticleMappingProfile()
        {
            CreateMap<BlogArticleEntity, BlogArticleResponse>();
            CreateMap<BlogArticleResponse, BlogArticleEntity>();
            CreateMap<BlogArticleEntity, BlogArticleUpsertRequest>();
            CreateMap<BlogArticleUpsertRequest, BlogArticleEntity>();
        }
    }
}