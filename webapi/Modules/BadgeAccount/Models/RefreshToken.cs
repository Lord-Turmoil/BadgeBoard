using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
	[Owned]
	public class RefreshToken
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		
		public string Token { get; set; }

		public DateTime Expires { get; set; }

		public bool IsExpired => DateTime.UtcNow > Expires;


		public DateTime Created { get; set; }
		public DateTime? Revoked { get; set; }

		public bool IsActive => Revoked == null && !IsExpired;
	}
}
