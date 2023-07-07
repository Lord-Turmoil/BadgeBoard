namespace BadgeBoard.Api.Extensions.Response
{
	public class ApiResponseDto
	{
		public ApiResponseMeta Meta { get; set; }
		public object? Data { get; set; }

		protected ApiResponseDto(int status, string? message = null, object? data = null)
		{
			Meta = new ApiResponseMeta(status, message);
			Data = data;
		}
	}

	public class ApiResponseMeta
	{
		public int Status { get; set; }
		public string? Message { get; set; }

		public ApiResponseMeta()
		{
			Status = 0;
			Message = null;
		}

		public ApiResponseMeta(int status, string? message = null)
		{
			Status = status;
			Message = message;
		}
	}
}
