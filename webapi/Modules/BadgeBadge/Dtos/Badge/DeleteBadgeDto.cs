using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
	public class DeleteBadgeDto : IApiRequestDto
	{
		public List<int> Badges { get; set; } = new List<int>();

		// Admin can force delete a badge. :)
		public bool Force { get; set; }

		public bool Verify()
		{
			return true;
		}

		public IApiRequestDto Format()
		{
			return this;
		}
	}

	public class DeleteBadgeErrorData
	{
		public int Id { get; set; }
		public string? Message { get; set; }
	}

	public class DeleteBadgeSuccessDto : ApiResponseData
	{
		public List<DeleteBadgeErrorData> Errors = new();
	}
}
