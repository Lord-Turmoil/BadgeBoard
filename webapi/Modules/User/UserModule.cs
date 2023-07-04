using BadgeBoard.Api.Extensions.Module;

namespace BadgeBoard.Api.Modules.User
{
	public class UserModule : BadgeModule
	{
		public new IServiceCollection RegisterModule(IServiceCollection services)
		{
			return services;
		}
	}
}
