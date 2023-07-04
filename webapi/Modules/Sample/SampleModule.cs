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
			ConfigureDatabase<SampleContext>(services);

			services.AddUnitOfWork<SampleContext>()
				.AddCustomRepository<Weather, WeatherRepository>();

			return services;
		}
	}
}
