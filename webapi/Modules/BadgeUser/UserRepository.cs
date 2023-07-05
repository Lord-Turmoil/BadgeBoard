using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeUser
{
	public class UserRepository : Repository<User>
	{
		public UserRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}

	public class UserPreferenceRepository : Repository<UserPreference>
	{
		public UserPreferenceRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}

	public class UserInfoRepository : Repository<UserInfo>
	{
		public UserInfoRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}

	public class FavoriteUserRepository : Repository<FavoriteUser>
	{
		public FavoriteUserRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}
}
