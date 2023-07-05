using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Extensions.Response
{
	public class ApiResponse : JsonResult
	{
		public ApiResponse(int code, object? value) : base(value)
		{
			StatusCode = code;
		}
	}
}
