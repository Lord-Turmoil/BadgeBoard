using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeBoard.Api.Modules.BadgeBadge.Models
{
	public class UnreadRecord
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int UserId { get; set; }

		public int QuestionCount { get; set; }
		public int MemoryCount { get; set; }
	}
}
