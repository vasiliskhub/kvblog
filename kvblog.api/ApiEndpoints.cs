namespace Kvblog.Api
{
	public class ApiEndpoints
	{
		private const string ApiBase = "api/v1";

		public static class BlogArticles
		{
			private const string BasePath = $"{ApiEndpoints.ApiBase}/blogarticles";

			public const string GetById = $"{BasePath}/{{id:guid}}";
			public const string GetBySlug = $"{BasePath}/{{slug}}";
			public const string GetAll = $"{BasePath}";
			public const string Create = $"{BasePath}";
			public const string Update = $"{BasePath}/{{id:guid}}";
			public const string Delete = $"{BasePath}/{{id:guid}}";
			public const string Search = $"{BasePath}/search";
		}
	}
}
