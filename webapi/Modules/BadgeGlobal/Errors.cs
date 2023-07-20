namespace BadgeBoard.Api.Modules.BadgeGlobal
{
	public static class Errors
	{
		// Internal server errors
		public const int FailedToSaveChanges = 6601;
		public const int MissingReference = 6602;

		// User status
		public const int UserAlreadyExists = 1001;
		public const int UserNotExists = 1002;
		public const int BadUserJwt = 1003;

		// Login
		public const int WrongPassword = 2001;
		public const int GetTokenRejected = 2002;
		public const int RefreshTokenRejected = 2003;
		public const int RevokeTokenRejected = 2004;

		// User Info
		public const int DeleteAvatarError = 3001;
		public const int SaveAvatarError = 3002;

		// Category
		public const int CategoryAlreadyExists = 4001;

	}
}
