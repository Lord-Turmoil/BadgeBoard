// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class LoginDto : IApiRequestDto
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public bool Verify()
		{
			return !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password));
		}

		public IApiRequestDto Format()
		{
			Username = Username.Trim();
			Password = Password.Trim();
			return this;
		}
	}

	public class LoginWrongPasswordDto : OrdinaryDto
	{
		public LoginWrongPasswordDto() : base(Errors.WrongPassword, "Wrong password")
		{
		}
	}
}
