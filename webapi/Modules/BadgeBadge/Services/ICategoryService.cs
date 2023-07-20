using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services
{
	/// <summary>
	/// Creation and deletion of category. id is from the request
	/// authorization, denoting the user who is making change of his/her
	/// own categories.
	/// </summary>
	public interface ICategoryService
	{
		public Task<ApiResponse> Exists(int id, string name);

		public Task<ApiResponse> AddCategory(int id, AddCategoryDto dto);

		// Delete a category, can optionally merge data into another.
		public Task<ApiResponse> DeleteCategory(int id);

		// Only rename the category.
		public Task<ApiResponse> RenameCategory(int id);

		// Update category options. Won't affect category name and
		// current badges in it.
		public Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDto dto);

		// Merge two categories, and abandon (delete) the merge source.
		// For now, will ignore target category options. :(
		public Task<ApiResponse> MergeCategory(int id);
	}
}
