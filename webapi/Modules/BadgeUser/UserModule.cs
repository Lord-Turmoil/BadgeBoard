using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace BadgeBoard.Api.Modules.BadgeUser
{
	public class UserModule : BaseModule
	{
		public override IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<User, UserRepository>()
				.AddCustomRepository<UserPreference, UserPreferenceRepository>()
				.AddCustomRepository<UserInfo, UserInfoRepository>()
				.AddCustomRepository<FavoriteUser, FavoriteUserRepository>();

			services.AddTransient<ILoginService, LoginService>()
				.AddTransient<IRegisterService, RegisterService>();

			return services;
		}
	}
}
