using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
	public class EmailRecord
	{
		public enum EmailType : int
		{
			Invalid,
			Register,
			Retrieve
		}

		[Key]
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public DateTime Expire { get; set; }

		[Required]
		[Column(TypeName = "varchar(15)")]
		public string Code { get; set; } = string.Empty;

		[Required]
		public int Type { get; set; } = (int)EmailType.Invalid;

		[Required]
		public bool IsValid { get; set; } = true;
	}
}
