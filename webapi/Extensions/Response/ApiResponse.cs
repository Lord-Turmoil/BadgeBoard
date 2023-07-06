using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Extensions.Response
{
	public class ApiResponse : JsonResult
	{
		protected ApiResponse(int code, object? value) : base(value)
		{
			StatusCode = code;
		}
	}
}
