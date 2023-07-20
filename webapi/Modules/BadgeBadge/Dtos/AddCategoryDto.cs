using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class AddCategoryDto : CategoryDto, IApiRequestDto
	{
		public bool Verify()
		{
			return Name.Length is > 0 and < Globals.MaxCategoryNameLength;
		}

		public IApiRequestDto Format()
		{
			Name = Name.Trim();
			return this;
		}
	}
}
