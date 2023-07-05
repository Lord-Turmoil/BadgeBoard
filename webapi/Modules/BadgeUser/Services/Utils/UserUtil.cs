using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Services.Utils
{
	public static class UserUtil
	{
		public static async Task<User?> GetUserByEmailAsync(IRepository<User> repo, string email)
		{
			return await repo.GetFirstOrDefaultAsync(predicate: x => x.Account.Email.Equals(email, StringComparison.Ordinal));
		}

		public static User? GetUserByEmail(IRepository<User> repo, string email)
		{
			return repo.GetFirstOrDefault(predicate: x => x.Account.Email.Equals(email, StringComparison.Ordinal));
		}

		public static async Task<User?> GetUserByUsernameAsync(IRepository<User> repo, string username)
		{
			return await repo.GetFirstOrDefaultAsync(predicate: x => x.Username.Equals(username, StringComparison.Ordinal));
		}

		public static User? GetUserByUsername(IRepository<User> repo, string username)
		{
			return repo.GetFirstOrDefault(predicate: x => x.Username.Equals(username, StringComparison.Ordinal));
		}
	}
}
