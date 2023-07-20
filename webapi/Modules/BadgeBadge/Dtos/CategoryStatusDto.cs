using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class CategoryAlreadyExistsDto : OrdinaryDto
	{
		public CategoryAlreadyExistsDto(string? message = null) : base(Errors.CategoryAlreadyExists, message)
		{
		}
	}
}
