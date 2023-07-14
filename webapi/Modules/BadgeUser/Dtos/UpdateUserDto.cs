using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class UpdateUserPreferenceDto
	{
		public bool? IsDefaultPublic { get; set; }
	}


	public class UpdateUserInfoDto
	{
		public string? Motto { get; set; }
		public string? Birthday { get; set; }
		public int? Sex { get; set; }
	}

	public class UpdateUsernameDto
	{
		public string? Username { get; set; }
	}
}
