// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services;

public interface ILoginService
{
	public Task<ApiResponse> Login(LoginDto dto);

	public Task<TokenResponseData> GetToken(TokenDto dto);

	public Task<TokenResponseData> RefreshToken(string token);

	public Task<RevokeTokenData> RevokeToken(string oldToken);
}