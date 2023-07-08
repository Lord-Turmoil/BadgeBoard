using BadgeBoard.Api.Extensions.Response;
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
	}

	public class LoginWrongPasswordDto : OrdinaryDto
	{
		public LoginWrongPasswordDto() : base(2001, "Wrong password")
		{
		}
	}

	// Login success dto
	public class LoginSuccessData : ApiResponseData
	{
		public UserDto User { get; set; }
		public string Jwt { get; set; }
	}

	public class LoginSuccessDto : GoodDto
	{
		public LoginSuccessDto(UserDto user, string jwt) : base("Welcome back, my friend")
		{
			Data = new LoginSuccessData { User = user, Jwt = jwt };
		}
	}
}
