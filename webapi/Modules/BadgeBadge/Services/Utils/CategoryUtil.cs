using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils
{
	public static class CategoryUtil
	{
		public static async Task<IList<Category>> GetCategoryOfUserAsync(IRepository<Category> repo, int id)
		{
			return await repo.GetAllAsync(predicate: x => x.UserId == id);
		}
	}
}
