using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class RegisterDto : ApiRequestDto
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public override bool Verify()
		{
			if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)) {
				return false;
			}

			return AccountVerifier.VerifyAccount(Password, Username);
		}
	}
}
