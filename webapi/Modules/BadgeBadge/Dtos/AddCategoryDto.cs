using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class AddCategoryDto : ApiRequestDto
	{
		public string Name { get; set; }

		public override bool Verify()
		{
			return Name.Length is > 0 and < Globals.MaxCategoryNameLength;
		}

		public override AddCategoryDto Format()
		{
			Name = Name.Trim();
			return this;
		}
	}
}
