using System.Security.Cryptography;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount.Services.Utils
{
	public static class TokenUtil
	{
		public const int RefreshTokenExpire = 14;
		public const string RefreshTokenCookiesName = "refreshToken";

		public static RefreshToken CreateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var generator = new RNGCryptoServiceProvider()) {
				generator.GetBytes(randomNumber);
			}

			return new RefreshToken {
				Token = Convert.ToBase64String(randomNumber),
				Expires = DateTime.UtcNow.AddDays(RefreshTokenExpire),
				Created = DateTime.UtcNow
			};
		}

		public static CookieOptions GetRefreshTokenCookieOptions()
		{
			return new CookieOptions {
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddDays(RefreshTokenExpire)
			};
		}
	}
}
