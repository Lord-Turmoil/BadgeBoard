using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

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
}
