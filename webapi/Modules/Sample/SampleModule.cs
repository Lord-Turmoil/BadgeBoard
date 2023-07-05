using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.Sample.Models;

namespace BadgeBoard.Api.Modules.Sample
{
    public class SampleModule : BaseModule
	{
		public override IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<Weather, WeatherRepository>();

			return services;
		}
	}
}
