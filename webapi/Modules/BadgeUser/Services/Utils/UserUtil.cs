using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Services.Utils
{
	public static class UserUtil
	{
		public static async Task<User?> GetUserByEmailAsync(IRepository<User> repo, string email)
		{
			return await repo.GetFirstOrDefaultAsync(predicate: x => x.Account.Email.Equals(email));
		}

		public static User? GetUserByEmail(IRepository<User> repo, string email)
		{
			return repo.GetFirstOrDefault(predicate: x => x.Account.Email.Equals(email));
		}

		public static async Task<User?> GetUserByUsernameAsync(IRepository<User> repo, string username)
		{
			return await repo.GetFirstOrDefaultAsync(predicate: x => x.Username.Equals(username));
		}

		public static User? GetUserByUsername(IRepository<User> repo, string username)
		{
			return repo.GetFirstOrDefault(predicate: x => x.Username.Equals(username));
		}

		public static bool HasUserByUsername(IRepository<User> repo, string username)
		{
			return repo.Exists(selector: x => x.Username.Equals(username));
		}

		public static async Task<bool> HasUserByUsernameAsync(IRepository<User> repo, string username)
		{
			return await repo.ExistsAsync(selector: x => x.Username.Equals(username));
		}

		public static User CreateUser(IUnitOfWork unitOfWork, RegisterDto dto)
		{
			AccountUtil.CreatePasswordHash(dto.Password, out byte[] salt, out byte[] hash);
			var account = UserAccount.Create(unitOfWork.GetRepository<UserAccount>(), salt, hash);
			var preference = UserPreference.Create(unitOfWork.GetRepository<UserPreference>());
			var info = UserInfo.Create(unitOfWork.GetRepository<UserInfo>());
			var user = User.Create(unitOfWork.GetRepository<User>(), dto.Username, account, preference, info);

			return user;
		}

		public static void EraseUser(IUnitOfWork unitOfWork, User user)
		{
			unitOfWork.GetRepository<UserPreference>().Delete(user.UserPreferenceId);
			unitOfWork.GetRepository<UserInfo>().Delete(user.UserInfoId);
			unitOfWork.GetRepository<UserAccount>().Delete(user.Id);
			// unitOfWork.GetRepository<User>().Delete(user);
		}
	}
}
