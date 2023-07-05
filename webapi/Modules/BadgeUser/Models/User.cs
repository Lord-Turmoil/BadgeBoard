using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeBoard.Api.Modules.BadgeUser.Models
{
	public class User
	{
		[Key]
		[ForeignKey(nameof(UserAccount))]
		public Guid Id { get; set; }

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

		// Foreign keys
		[ForeignKey(nameof(UserPreference))]
		public UserPreference Preference { get; set; }

		[ForeignKey(nameof(UserInfo))]
		public UserInfo Info { get; set; }
	}

	public class UserPreference
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public bool IsDefaultPublic { get; set; } = false;

		public static UserPreference Create(IRepository<UserPreference> repo)
		{
			return repo.Insert(new UserPreference());
		}
	}

	public static class UserSex
	{
		public const int Unknown = 0;
		public const int Male = 1;
		public const int Femail = 2;
	}

	public class UserInfo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string? Motto { get; set; }
		public string? Birthday { get; set; }
		public int Sex { get; set; } = UserSex.Unknown;

		public static UserInfo Create(IRepository<UserInfo> repo)
		{
			return repo.Insert(new UserInfo());
		}
	}
}
