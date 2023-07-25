// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser
{
	public class UserRepository : Repository<User>
	{
		public UserRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class UserPreferenceRepository : Repository<UserPreference>
	{
		public UserPreferenceRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class UserInfoRepository : Repository<UserInfo>
	{
		public UserInfoRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class FavoriteUserRepository : Repository<FavoriteUser>
	{
		public FavoriteUserRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}
}
