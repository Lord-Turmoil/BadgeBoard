namespace BadgeBoard.Api.Extensions.Response
{
	public class GoodResponse : BadgeResponse
	{
		public GoodResponse(object? value) : base(StatusCodes.Status200OK, value)
		{
		}
	}

	public class BadRequestResponse : BadgeResponse
	{
		public BadRequestResponse(object? value) : base(StatusCodes.Status400BadRequest, value)
		{
		}
	}
}
