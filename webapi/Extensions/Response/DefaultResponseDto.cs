namespace BadgeBoard.Api.Extensions.Response
{
	// Dto that does have data
	public class OrdinaryDto : ApiResponseData
	{
		public OrdinaryDto(int status, string? message = null) : base(status, message, null)
		{
		}
	}

	public class BadDto : ApiResponseData
	{
		public BadDto(int status, string message = "Bad request", object? data = null) : base(status, message, data)
		{
		}
	}

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

	public class InternalServerErrorDto : ApiResponseData
	{
		public InternalServerErrorDto(string message = "Unexpected error", object? data = null) : base(-2, message, data)
		{
		}
	}
}
