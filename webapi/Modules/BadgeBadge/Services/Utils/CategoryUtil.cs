using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;

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

		public static Category UpdateCategory(Category category, CategoryDto dto)
		{
			var updated = false;

			if (!dto.Name.Equals(category.Name)) {
				category.Name = dto.Name;
				updated = true;
			}

			if (dto.Option.IsPublic != category.Option.IsPublic) {
				category.Option.IsPublic = dto.Option.IsPublic;
				updated = true;
			}

			if (dto.Option.AllowAnonymity != category.Option.AllowAnonymity) {
				category.Option.AllowAnonymity = dto.Option.AllowAnonymity;
				updated = true;
			}

			if (dto.Option.AllowQuestion != category.Option.AllowQuestion) {
				category.Option.AllowQuestion = dto.Option.AllowQuestion;
				updated = true;
			}

			if (dto.Option.IsPublic != category.Option.AllowMemory) {
				category.Option.AllowMemory = dto.Option.AllowMemory;
				updated = true;
			}

			if (updated) {
				category.UpdatedTime = DateTime.Now;
			}

			return category;
		}

		public static async Task<List<DeleteBadgeErrorData>> DeleteCategoryAsync(IUnitOfWork unitOfWork, Category category, User user)
		{
			var badges = await unitOfWork.GetRepository<Badge>().GetAllAsync(predicate: x => x.CategoryId == category.Id);

			var errors = BadgeUtil.DeleteBadges(unitOfWork, badges, user, true);

			var repo = unitOfWork.GetRepository<Category>();
			repo.Delete(category);

			return errors;
		}

		// src and dst should not be both null (although cause redundant work only)
		public static async Task MergeCategoriesAsync(IUnitOfWork unitOfWork, Category? src, Category? dst)
		{
			var repo = unitOfWork.GetRepository<Badge>();

			IList<Badge> badges;
			if (src == null) {
				badges = await repo.GetAllAsync(predicate: x => x.CategoryId == null);
			} else {
				badges = await repo.GetAllAsync(predicate: x => x.CategoryId == src.Id);
			}

			foreach (var badge in badges) {
				badge.CategoryId = dst?.Id;
			}

			if (dst != null) {
				dst.UpdatedTime = DateTime.Now;
			}
		}
	}
}
