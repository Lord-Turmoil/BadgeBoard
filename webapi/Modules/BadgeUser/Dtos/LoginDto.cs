﻿using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class LoginDto : ApiRequestDto
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public override bool Verify()
		{
			return !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password));
		}

		public override LoginDto Format()
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

	// Login success dto
	public class LoginSuccessData : ApiResponseData
	{
		public UserCompleteDto User { get; set; }
		public string Jwt { get; set; }
	}

	public class LoginSuccessDto : GoodDto
	{
		public LoginSuccessDto(UserCompleteDto user, string jwt) : base("Welcome back, my friend")
		{
			Data = new LoginSuccessData { User = user, Jwt = jwt };
		}
	}
}
