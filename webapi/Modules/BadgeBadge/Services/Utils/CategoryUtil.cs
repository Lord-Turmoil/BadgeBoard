using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;
using BadgeBoard.Api.Modules.BadgeBadge.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils
{
	public static class CategoryUtil
	{
		public static async Task<bool> HasCategoryOfUserAsync(IRepository<Category> repo, int id, string name)
		{
			var category = await repo.GetFirstOrDefaultAsync(predicate: x => x.UserId == id && x.Name == name);
			return category != null;
		}

		public static async Task<IList<Category>> GetCategoryOfUserAsync(IRepository<Category> repo, int id)
		{
			return await repo.GetAllAsync(predicate: x => x.UserId == id);
		}

		public static Category UpdateCategory(Category category, UpdateCategoryDto dto)
		{
			if (!dto.Name.Equals(category.Name)) {
				category.Name = dto.Name;
			}

			if (dto.Option.IsPublic != category.Option.IsPublic) {
				category.Option.IsPublic = dto.Option.IsPublic;
			}

			if (dto.Option.AllowAnonymity != category.Option.AllowAnonymity) {
				category.Option.AllowAnonymity = dto.Option.AllowAnonymity;
			}

			if (dto.Option.AllowQuestion != category.Option.AllowQuestion) {
				category.Option.AllowQuestion = dto.Option.AllowQuestion;
			}

			if (dto.Option.IsPublic != category.Option.AllowMemory) {
				category.Option.AllowMemory = dto.Option.AllowMemory;
			}

			return category;
		}
	}
}
