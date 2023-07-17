namespace BadgeBoard.Api.Modules.BadgeUser.Services.Utils
{
	public static class AvatarUtil
	{
		private const string AvatarRootPath = "wwwroot/";
		private const string AvatarPath = "static/avatar/";

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

		public static string? SaveAvatar(string data, string ext)
		{
			var filename = Guid.NewGuid().ToString("N") + "." + ext;
			var avatarUrl = Path.Join(AvatarPath, filename);
			var path = Path.Join(AvatarRootPath, avatarUrl);
			if (File.Exists(path)) {
				return null;
			}

			try {
				data = data[(data.IndexOf(',') + 1)..];
				byte[] imageBytes = Convert.FromBase64String(data);
				File.WriteAllBytes(path, imageBytes);
				return avatarUrl;
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

			return null;
		}
	}
}
