using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class UpdateUserPreferenceDto : ApiRequestDto
	{
		public bool? IsDefaultPublic { get; set; }
	}


	public class UpdateUserInfoDto : ApiRequestDto
	{
		public string? Motto { get; set; }
		public string? Birthday { get; set; }
		public int? Sex { get; set; }

		public override bool Verify()
		{
			if (Motto is { Length: > Globals.MaxMottoLength }) {
				return false;
			}
			if (Sex != null && !UserSex.IsValid((int)Sex)) {
				return false;
			}
			return true;
		}

		public override UpdateUserInfoDto Format()
		{
			Motto = Motto?.Trim();
			Birthday = Birthday?.Trim();
			return this;
		}
	}

	public class UpdateUsernameDto : ApiRequestDto
	{
		public string? Username { get; set; }

		public override UpdateUsernameDto Format()
		{
			Username = Username?.Trim();
			return this;
		}
	}

	public class UpdateAvatarDto : ApiRequestDto
	{
		public string Extension { get; set; }
		public string Data { get; set; } // base 64 string
	}
}
