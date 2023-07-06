namespace BadgeBoard.Api.Extensions.Response
{
	public class GoodDto : ApiResponseData
	{
		public GoodDto(string message = "Nice request", object? data = null) : base(0, message, data)
		{
		}
	}

	public class BadRequestDto : ApiResponseData
	{
		public BadRequestDto(string message = "Request format error", object? data = null) : base(-1, message, data)
		{
		}
	}

	public class OrdinaryDto : ApiResponseData
	{
		public OrdinaryDto(int status, string? message = null) : base(status, message, null)
		{
		}
	}
}
