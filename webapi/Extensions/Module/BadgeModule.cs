using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BadgeModule : IModule
	{
		public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
		{
			return endpoints;
		}

		public IServiceCollection RegisterModule(IServiceCollection services)
		{
			return services;
		}

		public void ConfigureDatabase<TContext>(IServiceCollection services) where TContext : DbContext
		{
			var config = services.BuildServiceProvider()
				.GetService<IConfiguration>() ?? throw new Exception("Missing configuration");
			string profile = config["Profile"] ?? "Default";
			string database = config.GetConnectionString("Database") ?? throw new Exception("Missing database");
			string connection = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Missing database connection");

			if (database == "MySQL") {
				services.AddDbContext<TContext>(option => {
					option.UseMySQL(connection);
				});
			} else if (database == "SQLite") {
				services.AddDbContext<TContext>(option => {
					option.UseSqlite(connection);
				});
			} else {
				throw new Exception($"Invalid database: {database}");
			}
		}
	}
}
