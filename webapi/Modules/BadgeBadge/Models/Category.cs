using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeGlobal.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Models
{
	public class Category : TimeRecordModel
	{
		[Key]
		public int Id { get; init; }

		[Column(TypeName = "varchar(127)")]
		[Required]
		public string Name { get; set; }

		public int Size { get; set; } = 0;

		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }

		public static async Task<Category> CreateAsync(IRepository<Category> repo, string Name, User user)
		{
			var entry = await repo.InsertAsync(new Category {
				Id = KeyGenerator.GenerateKey(),
				Name = Name,
				Size = 0,
				User = user,
				CreatedTime = DateTime.Now,
				UpdatedTime = DateTime.Now
			});
			return entry.Entity;
		}
	}
}
