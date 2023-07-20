using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category
{
    public class UpdateCategoryDto : CategoryDto, IApiRequestDto
    {
        public int Id { get; set; }

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
