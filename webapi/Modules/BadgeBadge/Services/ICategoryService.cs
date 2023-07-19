using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services
{
	/// <summary>
	/// Creation and deletion of category.
	/// </summary>
	public interface ICategoryService
	{
		public Task<ApiResponse> AddCategory(int id);
	}
}
