// Verify account related values

using System.Text.RegularExpressions;

namespace BadgeBoard.Api.Modules.BadgeAccount.Services
{
	public class AccountVerifier
	{
		private AccountVerifier() { }

		public static bool VerifyPassword(string password)
		{
			return Regex.IsMatch(password, @"^(?=.*\d)(?=(.*\W){1})(?=.*[a-zA-Z])(?!.*\s).{1,16}$", RegexOptions.IgnoreCase);
		}

		public static bool VerifyEmail(string email)
		{
			return Regex.IsMatch(email, @"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$", RegexOptions.IgnoreCase);
		}

		public static bool VerifyAccount(string password, string email)
		{
			return VerifyPassword(password) && VerifyEmail(email);
		}
	}
}
