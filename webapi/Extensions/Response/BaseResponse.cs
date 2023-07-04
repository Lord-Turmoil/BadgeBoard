using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Extensions.Response
{
	public class BadgeResponse : JsonResult
	{
		public BadgeResponse(int code, object? value) : base(value)
		{
			StatusCode = code;
		}
	}
}
