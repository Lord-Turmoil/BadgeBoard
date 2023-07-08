namespace BadgeBoard.Api.Modules.BadgeGlobal
{
	public static class Errors
	{
		// User status
		public const int UserAlreadyExists = 1001;
		public const int UserNotExists = 1002;

		// Login
		public const int WrongPassword = 2001;
		public const int GetTokenRejected = 2002;
		public const int RefreshTokenRejected = 2003;
	}
}
