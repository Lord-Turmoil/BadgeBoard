// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Text.Json.Serialization;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using Google.Protobuf.Reflection;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class TokenDto : IApiRequestDto
	{
		public int Id { get; set; }
		public string Password { get; set; }

		public bool Verify()
		{
			return !(string.IsNullOrEmpty(Id.ToString()) || string.IsNullOrEmpty(Password));
		}

		public IApiRequestDto Format()
		{
			return this;
		}
	}

	public class TokenResponseData : ApiResponseData
	{
		public int Id { get; set; }
		public string Token { get; set; }

		[JsonIgnore] // will not present in response
		public string RefreshToken { get; set; }

		public DateTime RefreshTokenExpiration { get; set; }

		// Extra information
		[JsonIgnore] public bool IsAuthenticated { get; set; }

		[JsonIgnore] public int Status { get; set; } = StatusCodes.Status200OK;

		[JsonIgnore] public string? Message { get; set; }
	}

	public class GetTokenFailedDto : OrdinaryDto
	{
		public GetTokenFailedDto(string? message = "Not authorized") : base(Errors.GetTokenRejected, message)
		{
		}
	}

	public class RefreshTokenFailedDto : OrdinaryDto
	{
		public RefreshTokenFailedDto(string? message = "Not authorized") : base(Errors.RefreshTokenRejected, message)
		{
		}
	}

	public class RevokeTokenData : ApiResponseData
	{
		public bool Succeeded { get; set; }
		public int Status { get; set; } = StatusCodes.Status200OK;
		public string? Message { get; set; }
	}

	public class RevokeTokenFailedDto : OrdinaryDto
	{
		public RevokeTokenFailedDto(string? message = "Not authorized") : base(Errors.RevokeTokenRejected, message)
		{
		}
	}
}
