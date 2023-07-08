using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
	[Owned]
	public class UserAccount
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Column(TypeName = "varchar(63)")]
		[EmailAddress]
		[Required]
		public string Email { get; set; } = string.Empty;

		[Column(TypeName = "varbinary(256)")]
		[Required]
		public byte[] Password { get; set; } = Array.Empty<byte>();

		[Column(TypeName = "varbinary(256)")]
		[Required]
		public byte[] Salt { get; set; } = Array.Empty<byte>();

		public static async Task<UserAccount> CreateAsync(IRepository<UserAccount> repo, byte[] salt, byte[] password, string email = "")
		{
			var entry = await repo.InsertAsync(new UserAccount { Salt = salt, Password = password, Email = email });
			return entry.Entity;
		}

		public static async Task<UserAccount> GetAsync(IRepository<UserAccount> repo, Guid id)
		{
			return await repo.FindAsync(id);
		}
	}
}
