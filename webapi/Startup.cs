using BadgeBoard.Api.Extensions.CORS;
using BadgeBoard.Api.Extensions.Module;
using Microsoft.AspNetCore.Builder;
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
			services.AddCors();

			services.RegisterModules();

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
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			// Shows UseCors with CorsPolicyBuilder.
			// Must be placed between UseRouting and UseEndpoints
			app.UseCors(policy => {
				policy.AllowAnyOrigin()
					   .AllowAnyMethod()
					   .AllowAnyHeader();
			});

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapSwagger();
			});
		}
	}
}
