using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
	public class UpdateBadgeDto : IApiRequestDto
	{
		public int Id { get; set; }
		public string? Style { get; set; }
		public bool IsPublic { get; set; }

		public bool Verify()
		{
			if (Style is { Length: > Globals.MaxStyleLength }) {
				return false;
			}

			return true;
		}

		public IApiRequestDto Format()
		{
			Style = Style?.Trim();
			return this;
		}
	}

	public class UpdateBadgeSuccessDto : ApiResponseData
	{
		public string? Style { get; set; }
		public bool IsPublic { get; set; }
	}
}
