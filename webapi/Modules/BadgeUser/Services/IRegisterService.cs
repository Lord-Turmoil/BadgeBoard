// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IRegisterService
	{
		public ApiResponse SendCode(VerificationCodeDto dto);

		public Task<ApiResponse> Register(RegisterDto dto);

		public Task<ApiResponse> Cancel(CancelDto dto);
	}
}
