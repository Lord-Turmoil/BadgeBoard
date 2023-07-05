using System.ComponentModel.DataAnnotations;

namespace BadgeBoard.Api.Modules.BadgeUser.Models
{
	public class FavoriteUser
	{
		[Key]
		public int Id { get; set; }

		public Guid SrcId { get; set; }
		public Guid DstId { get; set; }
	}
}
