using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class CategoryAlreadyExistsDto : OrdinaryDto
	{
		public CategoryAlreadyExistsDto(string? message = "Category name already exists") : base(Errors.CategoryAlreadyExists, message)
		{
		}
	}

	public class CategoryNotExistsDto : OrdinaryDto
	{
		public CategoryNotExistsDto(string? message = "Category does not exit") : base(Errors.CategoryNotExits, message)
		{
		}
	}
}
