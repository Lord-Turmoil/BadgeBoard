using System.Security.Cryptography;

namespace BadgeBoard.Api.Modules.BadgeAccount.Services
{
	public static class AccountGenerator
	{
		public static void CreatePasswordHash(string password, out byte[] salt, out byte[] hash)
		{
			using (var hmac = new HMACSHA256()) {
				salt = hmac.Key;
				hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
	}
}
