using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Cors;
using BadgeBoard.Api.Extensions.Email;
using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BadgeBoard.Api
{
	public class Startup
	{
		private IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			ConfigureDatabase<BadgeContext>(services);
			services.AddUnitOfWork<BadgeContext>();
			services.RegisterModules();

			// Controllers
			services.AddControllers();

			// Swagger service
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "BadgeBoard.Api", Version = "v1" });
			});

			// Email options
			services.Configure<EmailOptions>(options => Configuration.GetSection(EmailOptions.EmailSection).Bind(options));

			// JWT options
			var jwtOptions = new JwtOptions();
			Configuration.GetRequiredSection(JwtOptions.JwtSection).Bind(jwtOptions);
			services.Configure<JwtOptions>(options => Configuration.GetSection(JwtOptions.JwtSection).Bind(options));
			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => {
				options.RequireHttpsMetadata = false;
				options.SaveToken = false;
				options.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,

					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Key)),
					ValidIssuer = jwtOptions.Issuer,
					ValidAudience = jwtOptions.Audience
				};
			});

			// AutoMapper
			var autoMapperConfig = new MapperConfiguration(config => {
				config.AddProfile(new AutoMapperProfile());
			});
			services.AddSingleton(autoMapperConfig.CreateMapper());

			// CORS
			var corsOptions = new CorsOptions();
			Configuration.GetRequiredSection(CorsOptions.CorsSection).Bind(corsOptions);
			if (corsOptions.Enable) {
				services.AddCors(options => {
					options.AddPolicy(
						name: CorsOptions.CorsPolicyName,
						policy => {
							policy.AllowAnyHeader().AllowAnyMethod();
							foreach (var origin in corsOptions.Origins) {
								policy.WithOrigins(origin);
							}
						});
				});
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BadgeBoard.Api v1"));
			}
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			// Enable static files. (before use cors)
			app.UseStaticFiles();

			// Must be placed between UseRouting and UseEndpoints
			app.UseCors(CorsOptions.CorsPolicyName);

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapSwagger();
			});
		}

		private void ConfigureDatabase<TContext>(IServiceCollection services) where TContext : DbContext
		{
			var profile = Configuration["Profile"] ?? "Default";
			var database = Configuration.GetConnectionString("Database") ?? throw new Exception("Missing database");
			var connection = Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Missing database connection");

			Console.WriteLine($"   Profile: {profile}");
			Console.WriteLine($"  Database: {database}");
			Console.WriteLine($"Connection: {connection}");

			switch (database) {
				case "MySQL":
					services.AddDbContext<TContext>(option => {
						option.UseMySQL(connection);
					});
					break;
				case "SQLite":
					services.AddDbContext<TContext>(option => {
						option.UseSqlite(connection);
					});
					break;
				default:
					throw new Exception($"Invalid database: {database}");
			}
		}
	}
}
