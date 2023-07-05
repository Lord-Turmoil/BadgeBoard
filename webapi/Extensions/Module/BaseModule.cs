using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BaseModule : IModule
	{
		public virtual IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
		{
			return endpoints;
		}

		public virtual IServiceCollection RegisterModule(IServiceCollection services)
		{
			return services;
		}
	}
}
