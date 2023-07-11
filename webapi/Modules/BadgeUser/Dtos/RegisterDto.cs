using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;
using Org.BouncyCastle.Asn1.Mozilla;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class RegisterDto : ApiRequestDto
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public override bool Verify()
		{
			return AccountVerifier.VerifyAccount(Username, Password);
		}
	}

	internal class RegisterSuccessData : ApiResponseData
	{
		public UserCompleteDto User { get; set; }
	}

	public class RegisterSuccessDto : GoodDto
	{
		public RegisterSuccessDto(UserCompleteDto user) : base("Welcome, my friend!")
		{
			Data = new RegisterSuccessData { User = user };
		}
	}
}
