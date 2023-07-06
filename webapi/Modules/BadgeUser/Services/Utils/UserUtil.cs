using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services;
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

	
		public static User CreateUser(IUnitOfWork unitOfWork, RegisterDto dto)
		{
			AccountGenerator.CreatePasswordHash(dto.Password, out byte[] salt, out byte[] hash);
			var account = UserAccount.Create(unitOfWork.GetRepository<UserAccount>(), salt, hash);
			var preference = UserPreference.Create(unitOfWork.GetRepository<UserPreference>());
			var info = UserInfo.Create(unitOfWork.GetRepository<UserInfo>());
			var user = User.Create(unitOfWork.GetRepository<User>(), dto.Username, account, preference, info);

			return user;
		}
	}
}
