using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeGlobal.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Models
{
	public class Badge : TimeRecordModel
	{
		public static class Types
		{
			public const int Question = 1;
			public const int Memory = 2;

			public static bool IsValid(int type)
			{
				return type is Question or Memory;
			}
		}

		[Key]
		public int Id { get; init; }

		// Identify different payloads
		public int Type { get; set; }
		public int PayloadId { get; set; }

		// Users
		public int Sender { get; set; }
		public int Receiver { get; set; }

		// For CSS style class
		[Column(TypeName = "varchar(31)")] public string? Style { get; set; }

		// Restrictions
		public bool IsLocked { get; set; } = false;
		public bool IsPublic { get; set; } = true;

		// Foreign keys
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")] public Category Category { get; set; }

		public int UserId { get; set; }
		[ForeignKey("UserId")] public User User { get; set; }
	}

	public class QuestionPayload
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column(TypeName = "text")] public string Question { get; set; } = "";

		[Column(TypeName = "text")] public string? Answer { get; set; }

		public static async Task<QuestionPayload> CreateAsync(
			IRepository<QuestionPayload> repo,
			string question = "",
			string? answer = null)
		{
			var entry = await repo.InsertAsync(new QuestionPayload {
				Question = question,
				Answer = answer
			});
			return entry.Entity;
		}
	}

	public class MemoryPayload
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column(TypeName = "text")]
		public string Memory { get; set; } = "";

		public static async Task<MemoryPayload> CreateAsync(IRepository<MemoryPayload> repo, string memory = "")
		{
			var entry = await repo.InsertAsync(new MemoryPayload { Memory = memory });
			return entry.Entity;
		}

		public static async Task<MemoryPayload?> GetAsync(IRepository<MemoryPayload> repo, int id)
		{
			return await repo.FindAsync(id);
		}
	}
}
