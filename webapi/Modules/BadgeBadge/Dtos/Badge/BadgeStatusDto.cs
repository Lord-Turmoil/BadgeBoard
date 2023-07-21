using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
	public class BadgeNotExistsDto : OrdinaryDto
	{
		public BadgeNotExistsDto(string? message = "Badge does no exists") : base(Errors.BadgeNotExists, message)
		{
		}
	}
}
