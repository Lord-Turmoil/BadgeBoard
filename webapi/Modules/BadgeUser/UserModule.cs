using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser
{
	public class UserModule : BaseModule
	{
		public new IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<User, UserRepository>()
				.AddCustomRepository<UserPreference, UserPreferenceRepository>()
				.AddCustomRepository<UserInfo, UserInfoRepository>()
				.AddCustomRepository<FavoriteUser, FavoriteUserRepository>();

			return services;
		}
	}
}
