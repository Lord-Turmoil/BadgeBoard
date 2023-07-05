using AutoMapper;
using BadgeBoard.Api.Extensions.Email;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules;
using BadgeBoard.Api.Modules.BadgeGlobal;
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
			Global.Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();

			ConfigureDatabase<BadgeContext>(services);
			services.AddUnitOfWork<BadgeContext>();
			services.RegisterModules();

			// Controllers
			services.AddControllers();

			// Swagger service
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo() { Title = "SimpleToDo.Api", Version = "v1" });
			});

			// Email options
			services.Configure<EmailOptions>(options => Configuration.GetSection(EmailOptions.EmailSection).Bind(options));

			// AutoMapper
			var autoMapperConfig = new MapperConfiguration(config => {
				config.AddProfile(new AutoMapperProfile());
			});
			services.AddSingleton(autoMapperConfig.CreateMapper());

			Global.Services = services;
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

		private void ConfigureDatabase<TContext>(IServiceCollection services) where TContext : DbContext
		{
			string profile = Configuration["Profile"] ?? "Default";
			string database = Configuration.GetConnectionString("Database") ?? throw new Exception("Missing database");
			string connection = Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Missing database connection");

			Console.WriteLine($"   Profile: {profile}");
			Console.WriteLine($"  Database: {database}");
			Console.WriteLine($"Connection: {connection}");

			if ("MySQL".Equals(database)) {
				services.AddDbContext<TContext>(option => {
					option.UseMySQL(connection);
				});
			} else if ("SQLite".Equals(database)) {
				services.AddDbContext<TContext>(option => {
					option.UseSqlite(connection);
				});
			} else {
				throw new Exception($"Invalid database: {database}");
			}
		}
	}
}
