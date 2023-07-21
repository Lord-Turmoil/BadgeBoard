using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
	public class MoveBadgeDto : IApiRequestDto
	{
		public int Id { get; set; }
		public int Category { get; set; }

		public bool Verify()
		{
			return true;
		}

		public IApiRequestDto Format()
		{
			return this;
		}
	}
}
