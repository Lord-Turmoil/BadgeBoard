namespace BadgeBoard.Api.Extensions.Response
{
	public class GoodResponse : ApiResponse
	{
		public GoodResponse(object? value) : base(StatusCodes.Status200OK, value)
		{
		}
	}

	public class BadRequestResponse : ApiResponse
	{
		public BadRequestResponse(object? value) : base(StatusCodes.Status400BadRequest, value)
		{
		}
	}

	public class UnauthorizedResponse : ApiResponse
	{
		public UnauthorizedResponse(object? value) : base(StatusCodes.Status401Unauthorized, value)
		{
		}
	}

	public class InternalServerErrorResponse : ApiResponse
	{
		public InternalServerErrorResponse(object? value) : base(StatusCodes.Status500InternalServerError, value)
		{
		}
	}
}
