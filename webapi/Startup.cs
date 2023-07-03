using BadgeBoard.Api.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BadgeBoard.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			string profile = Configuration["Profile"] ?? "Default";
			string connection = Configuration.GetConnectionString("Connection") ?? throw new Exception("Missing database connection");
			if ("Production".Equals(profile)) {
				services.AddDbContext<BadgeBoardContext>(option => {
					option.UseMySQL(connection);
				});
			} else {
				services.AddDbContext<BadgeBoardContext>(option => {
					option.UseSqlite(connection);
				});
			}

			services.AddControllers();
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo() { Title = "SimpleToDo.Api", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleToDo.Api v1"));
			}
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapSwagger();
			});
		}
	}
}
