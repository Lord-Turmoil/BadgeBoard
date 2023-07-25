// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;
using Org.BouncyCastle.Asn1.Mozilla;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class RegisterDto : IApiRequestDto
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public bool Verify()
		{
			return AccountVerifier.VerifyAccount(Username, Password);
		}

		public IApiRequestDto Format()
		{
			Username = Username.Trim();
			Password = Password.Trim();
			return this;
		}
	}
}
