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

		public static User? GetUserById(IRepository<User> repo, Guid id)
		{
			return repo.Find(id);
		}

		public static async Task<User?> GetUserByIdAsync(IRepository<User> repo, Guid id)
		{
			return await repo.FindAsync(id);
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
			var user = repo.GetFirstOrDefault(predicate: x => x.Username.Equals(username));
			return user != null;
		}

		public static async Task<bool> HasUserByUsernameAsync(IRepository<User> repo, string username)
		{
			var user = await repo.GetFirstOrDefaultAsync(predicate: x => x.Username.Equals(username));
			return user != null;
		}

		public static bool HasUserById(IRepository<User> repo, Guid id)
		{
			var user = repo.Find(id);
			return user != null;
		}

		public static async Task<bool> HasUserByIdAsync(IRepository<User> repo, Guid id)
		{
			var user = await repo.FindAsync(id);
			return user != null;
		}

		public static async Task<User> CreateUserAsync(IUnitOfWork unitOfWork, RegisterDto dto)
		{
			AccountUtil.CreatePasswordHash(dto.Password, out byte[] salt, out byte[] hash);
			var account = await UserAccount.CreateAsync(unitOfWork.GetRepository<UserAccount>(), salt, hash);
			var preference = await UserPreference.CreateAsync(unitOfWork.GetRepository<UserPreference>());
			var info = await UserInfo.CreateAsync(unitOfWork.GetRepository<UserInfo>());
			var user = await User.CreateAsync(unitOfWork.GetRepository<User>(), dto.Username, account, preference, info);

			return user;
		}

		public static void EraseUser(IUnitOfWork unitOfWork, User user)
		{
			unitOfWork.GetRepository<UserPreference>().Delete(user.UserPreferenceId);
			unitOfWork.GetRepository<UserInfo>().Delete(user.UserInfoId);
			unitOfWork.GetRepository<UserAccount>().Delete(user.Id);
			// Cascade delete by UserAccount
			// unitOfWork.GetRepository<User>().Delete(user);
		}
	}
}
