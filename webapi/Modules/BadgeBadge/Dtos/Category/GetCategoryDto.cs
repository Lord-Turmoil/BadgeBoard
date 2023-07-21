using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category
{
	public class GetCategoryDto : ApiResponseData
	{
		public List<CategoryDto> Categories { get; set; } = new();
	}
}
