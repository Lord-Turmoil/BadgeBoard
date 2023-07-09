using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services;

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
				.AddTransient<IRegisterService, RegisterService>()
				.AddTransient<IUserService, UserService>();

			return services;
		}
	}
}
