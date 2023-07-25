// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
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

		public static User? FindUserByEmail(IRepository<User> repo, string email)
		{
			return repo.GetFirstOrDefault(predicate: x => x.Account.Email.Equals(email));
		}
		
		public static async Task<User?> FindUserByIdAsync(IRepository<User> repo, int id)
		{
			return await repo.FindAsync(id);
		}

		public static async Task<User?> FindUserByUsernameAsync(IRepository<User> repo, string username)
		{
			return await repo.GetFirstOrDefaultAsync(predicate: x => x.Username.Equals(username));
		}

		public static async Task<bool> HasUserByUsernameAsync(IRepository<User> repo, string username)
		{
			var user = await repo.GetFirstOrDefaultAsync(predicate: x => x.Username.Equals(username));
			return user != null;
		}

		public static async Task<bool> HasUserByIdAsync(IRepository<User> repo, int id)
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
			var unreadRecord = await UnreadRecord.CreateAsync(unitOfWork.GetRepository<UnreadRecord>());
			var user = await User.CreateAsync(unitOfWork.GetRepository<User>(), dto.Username, account, preference, info, unreadRecord);

			return user;
		}

		public static void EraseUser(IUnitOfWork unitOfWork, User user)
		{
			unitOfWork.GetRepository<User>().Delete(user);
			unitOfWork.GetRepository<UserAccount>().Delete(user.Id);
			unitOfWork.GetRepository<UserPreference>().Delete(user.UserPreferenceId);
			unitOfWork.GetRepository<UserInfo>().Delete(user.UserInfoId);
			unitOfWork.GetRepository<UnreadRecord>().Delete(user.UnreadRecordId);
		}
	}
}
