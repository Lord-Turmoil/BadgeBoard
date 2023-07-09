namespace BadgeBoard.Api.Extensions.Response
{
	// Dto that does have data
	public class OrdinaryDto : ApiResponseDto
	{
		public OrdinaryDto(int status, string? message = null) : base(status, message, null)
		{
		}
	}

	public class BadDto : ApiResponseDto
	{
		public BadDto(int status, string? message = "Bad request", object? data = null) : base(status, message, data)
		{
		}
	}

	public class GoodDto : ApiResponseDto
	{
		public GoodDto(string? message = "Nice request", object? data = null) : base(0, message, data)
		{
		}
	}

	public class GoodWithDataDto : GoodDto
	{
		public GoodWithDataDto(object? data = null) : base("Nice request", data)
		{
		}
	}

	public class BadRequestDto : ApiResponseDto
	{
		public BadRequestDto(string? message = "Request format error", object? data = null)
			: base(StatusCodes.Status400BadRequest, message, data)
		{
		}
	}

	public class UnauthorizedDto : ApiResponseDto
	{
		public UnauthorizedDto(string? message = "Who are you?", object? data = null) 
			: base(StatusCodes.Status401Unauthorized, message, data)
		{
		}
	}

	public class InternalServerErrorDto : ApiResponseDto
	{
		public InternalServerErrorDto(string? message = "Unexpected error", object? data = null) 
			: base(StatusCodes.Status500InternalServerError, message, data)
		{
		}
	}
}
