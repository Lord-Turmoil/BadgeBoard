﻿namespace BadgeBoard.Api.Extensions.Response
{
	public class ApiResponseData
	{
		public ApiResponseMeta Meta { get; set; }
		public object? Data { get; set; }

		public ApiResponseData(int status, string? message = null, object? data = null)
		{
			Meta = new ApiResponseMeta(status, message);
			Data = data;
		}
	}

	public class ApiResponseData<TData> where TData : class
	{
		public ApiResponseMeta Meta { get; set; }
		public TData? Data { get; set; }

		public ApiResponseData()
		{
			Meta = new ApiResponseMeta();
			Data = null;
		}

		public ApiResponseData(int status, string? message = null, TData? data = null)
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
