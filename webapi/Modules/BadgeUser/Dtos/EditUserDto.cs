using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class EditUserPreferenceDto
	{
		public bool? IsDefaultPublic { get; set; }
	}


	public class EditUserInfoDto
	{
		public string? Motto { get; set; }
		public string? Birthday { get; set; }
		public int? Sex { get; set; }
	}

}
