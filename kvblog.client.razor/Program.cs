using Auth0.AspNetCore.Authentication;
using Kvblog.Client.Razor.Services;
using Kvblog.Client.Razor.Utilities;
using Kvblog.Client.Razor.Utilities.AuthorizationRequirements;
using Kvblog.Client.Razor.Utilities.AuthPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Account/Logout");
    options.Conventions.AuthorizePage("/Account/Profile");
    options.Conventions.AuthorizeFolder("/Admin","IsAdmin");
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddHttpClient("KvblogApi",
      client => client.BaseAddress = new Uri(builder.Configuration["KvblogBaseApiUrl"]))
      .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("KvblogApi"));
builder.Services.AddScoped<IBlogArticleService>(sp =>
	new BlogArticleService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("KvblogApi")));
builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:KvblogDomain"];
        options.ClientId = builder.Configuration["Auth0:KvblogClientId"];
        options.ClientSecret = builder.Configuration["Auth0:KvblogClientSecret"];
        options.Scope = "openid profile email";
    }).WithAccessToken(options =>
    {
        options.Audience = builder.Configuration["Auth0:KvblogAudience"];
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy =>
        policy.Requirements.Add(new IsAdminRequirement()));
    options.AddPolicy("CUDAccess", policy =>
                      policy.RequireClaim("permissions", "kvblog:cud"));
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.Run();
