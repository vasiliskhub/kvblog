using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kvblog.Api.Db
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
	{
		public BlogDbContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.AddEnvironmentVariables()
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
			optionsBuilder.UseNpgsql(config.GetConnectionString("KvblogConnectionString"));

			return new BlogDbContext(optionsBuilder.Options);
		}
	}
}
