using AutoMapper;
using Kvblog.Api.Application.Mapping;
using Kvblog.Api.Application.Repositories;
using Kvblog.Api.Application.Services;
using Kvblog.Api.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//ToDo Tidy up registrations
builder.Services.AddSwaggerGen(
              options =>
              {

                  options.SwaggerDoc("v1", new OpenApiInfo { Title = "Kvblog Api", Version = "v1" });

                  options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                  {
                      Type = SecuritySchemeType.OAuth2,
                      Flows = new OpenApiOAuthFlows
                      {
                          Implicit = new OpenApiOAuthFlow
                          {
                              AuthorizationUrl = new Uri(builder.Configuration["Auth0:KvblogAuthorizationUrl"], UriKind.Absolute),
                              Scopes = new Dictionary<string, string>
                              {
                              }
                          }
                      }
                  });
                  options.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                      {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                          }
                          ,new string[] {}
                      }
                  });
              });

var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
optionsBuilder.UseNpgsql(builder.Configuration["ConnectionStrings:KvblogConnectionString"]);
await using var dbContext = new BlogDbContext(optionsBuilder.Options);

builder.Services.AddDbContext<BlogDbContext>(
    dbContextOptions => dbContextOptions.UseNpgsql(
        builder.Configuration["ConnectionStrings:KvblogConnectionString"]));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:KvblogAuthority"];
        options.Audience = builder.Configuration["Auth0:KvblogAudience"];
    });
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("CUDAccess", policy => policy.RequireClaim("permissions", "kvblog:cud"));
});

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<BlogArticleMappingProfile>();
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

//ToDo Move thes registrations to Application Layer
builder.Services.AddScoped<IBlogArticleRepository, BlogArticleRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(
                "An unexpected fault happened. Try again later.");
        });
    });
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kvblog Api V1");
    c.OAuthClientId(builder.Configuration["Auth0:KvblogApiClientId"]);
    c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", builder.Configuration["Auth0:KvblogAudience"] } });
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
