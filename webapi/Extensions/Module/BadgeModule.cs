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
	}
}
