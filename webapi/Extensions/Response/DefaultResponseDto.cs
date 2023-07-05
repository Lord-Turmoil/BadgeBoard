namespace BadgeBoard.Api.Extensions.Response
{
	public class GoodDto : ApiResponseDto
	{
		public GoodDto(string message = "Nice request", object? data = null) : base(0, message, data)
		{
		}
	}

	public class BadRequestDto : ApiResponseDto
	{
		public BadRequestDto(string message = "Request format error", object? data = null) : base(-1, message, data)
		{
		}
	}
}
