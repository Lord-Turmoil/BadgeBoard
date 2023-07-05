using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
	public class Account
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
	}
}
