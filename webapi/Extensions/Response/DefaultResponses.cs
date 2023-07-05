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
}
