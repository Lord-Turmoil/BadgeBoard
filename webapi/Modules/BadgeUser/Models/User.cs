using BadgeBoard.Api.Modules.BadgeAccount.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeUser.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Id")]
		public UserAccount Account { get; set; }

		// User self fields
		[Column(TypeName = "varchar(63)")]
		[Required]
		public string Username { get; set; }

		[Column(TypeName = "varchar(127)")]
		public string? AvatarUrl { get; set; }

		[Column(TypeName = "varchar(31)")]
		public string? Title { get; set; }

		public bool IsLocked { get; set; } = false;
		public bool IsAdmin { get; set; } = false;

		// foreign key property
		public int UserPreferenceId { get; set; }
		// reference navigation property
		[ForeignKey("UserPreferenceId")]
		public UserPreference Preference { get; set; }

		public int UserInfoId { get; set; }
		[ForeignKey("UserInfoId")]
		public UserInfo Info { get; set; }

		// Refresh tokens
		public List<RefreshToken> RefreshTokens { get; set; }

		public static async Task<User> CreateAsync(IRepository<User> repo, string username, UserAccount account, UserPreference preference, UserInfo info)
		{
			var entry = await repo.InsertAsync(new User {
				Username = username,
				Account = account,
				Preference = preference,
				Info = info
			});
			return entry.Entity;
		}

		public static async Task<User> GetAsync(IRepository<User> repo, Guid id)
		{
			return await repo.FindAsync(id);
		}

		/// <summary>
		/// Get all related entities, since the Arch guys didn't implement this.
		/// </summary>
		/// <param name="unitOfWork">Global unit of work</param>
		/// <param name="user">The user to include related entities</param>
		/// <returns></returns>
		public static async Task<User> IncludeAsync(IUnitOfWork unitOfWork, User user)
		{
			user.Account = await UserAccount.GetAsync(
				unitOfWork.GetRepository<UserAccount>(), user.Id);
			user.Preference = await UserPreference.GetAsync(
				unitOfWork.GetRepository<UserPreference>(), user.UserPreferenceId);
			user.Info = await UserInfo.GetAsync(
				unitOfWork.GetRepository<UserInfo>(), user.UserInfoId);

			return user;
		}
	}

	[Owned]
	public class UserPreference
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public bool IsDefaultPublic { get; set; } = false;

		public static async Task<UserPreference> CreateAsync(IRepository<UserPreference> repo)
		{
			var entry = await repo.InsertAsync(new UserPreference());
			return entry.Entity;
		}

		public static async Task<UserPreference> GetAsync(IRepository<UserPreference> repo, int id)
		{
			return await repo.FindAsync(id);
		}
	}

	public static class UserSex
	{
		public const int Unknown = 0;
		public const int Male = 1;
		public const int Female = 2;

		public static bool IsValid(int sex)
		{
			return sex is >= Unknown and <= Female;
		}
	}

	[Owned]
	public class UserInfo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column(TypeName = "varchar(127)")]
		public string? Motto { get; set; }

		[Column(TypeName = "varchar(15)")]
		public string? Birthday { get; set; }
		public int? Sex { get; set; }

		public static async Task<UserInfo> CreateAsync(IRepository<UserInfo> repo)
		{
			var entry = await repo.InsertAsync(new UserInfo());
			return entry.Entity;
		}

		public static async Task<UserInfo> GetAsync(IRepository<UserInfo> repo, int id)
		{
			return await repo.FindAsync(id);
		}
	}
}
