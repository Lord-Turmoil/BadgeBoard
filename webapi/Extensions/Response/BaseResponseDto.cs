namespace BadgeBoard.Api.Extensions.Response
{
	public class BaseResponseDto
	{
		public ResponseMeta Meta { get; set; }
		public object? Data { get; set; }

		public BaseResponseDto(int status, string? message = null, object? data = null)
		{
			Meta = new ResponseMeta(status, message);
			Data = data;
		}
	}

	public class BaseResponseDto<TData> where TData : class
	{
		public ResponseMeta Meta { get; set; }
		public TData? Data { get; set; }

		public BaseResponseDto()
		{
			Meta = new ResponseMeta();
			Data = null;
		}

		public BaseResponseDto(int status, string? message = null, TData? data = null)
		{
			Meta = new ResponseMeta(status, message);
			Data = data;
		}
	}

	public class ResponseMeta
	{
		public int Status { get; set; }
		public string? Message { get; set; }

		public ResponseMeta()
		{
			Status = 0;
			Message = null;
		}

		public ResponseMeta(int status, string? message = null)
		{
			Status = status;
			Message = message;
		}
	}
}
