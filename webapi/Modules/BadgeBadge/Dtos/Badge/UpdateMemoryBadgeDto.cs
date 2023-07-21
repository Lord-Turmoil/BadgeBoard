using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
	public class UpdateMemoryBadgeDto : IApiRequestDto
	{
		public int Id { get; set; }
		public string Memory { get; set; }

		public bool Verify()
		{
			return Memory.Length is > 0 and < Globals.MaxMemoryLength;
		}

		public IApiRequestDto Format()
		{
			Memory = Memory.Trim();
			return this;
		}
	}

	public class UpdateMemoryBadgeSuccessDto : ApiResponseData
	{
		public string Memory { get; set; }
	}
}
