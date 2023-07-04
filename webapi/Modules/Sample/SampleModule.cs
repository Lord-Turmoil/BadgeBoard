using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.Sample.Models;
using BadgeBoard.Api.Modules.Sample.Repository;

namespace BadgeBoard.Api.Modules.Sample
{
	public class SampleModule : BadgeModule
	{
		public new IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<Weather, WeatherRepository>();

			return services;
		}
	}
}
