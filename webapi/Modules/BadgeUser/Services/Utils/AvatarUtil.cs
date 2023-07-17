namespace BadgeBoard.Api.Modules.BadgeUser.Services.Utils
{
	public static class AvatarUtil
	{
		public const string AvatarRootPath = "wwwroot/";

		public static bool DeleteAvatar(string? avatarUrl)
		{
			if (avatarUrl == null) {
				return true;
			}

			var fullPath = Path.Join(AvatarRootPath, avatarUrl);
			if (!File.Exists(fullPath)) {
				return true;
			}

			try {
				File.Delete(fullPath);
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
				return false;
			}

			return true;
		}
	}
}
