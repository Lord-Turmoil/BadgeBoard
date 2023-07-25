// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

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
