// Reference: https://www.youtube.com/watch?v=v7q3pEK1EA0

using System.Security.Cryptography;

namespace BadgeBoard.Api.Modules.BadgeAccount.Services.Utils
{
    public static class AccountUtil
    {
	    private const int MinUserId = 1000000000;
        private const int MaxUserId = 2000000000;

        public static int GenerateAccountId()
        {
	        return (int)Random.Shared.NextInt64(MinUserId, MaxUserId + 1);
        }

        public static void CreatePasswordHash(string password, out byte[] salt, out byte[] hash)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public static bool VerifyPasswordHash(string password, byte[] salt, byte[] hash)
        {
            using var hmac = new HMACSHA256(salt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computeHash.SequenceEqual(hash);
        }
    }
}
