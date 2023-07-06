﻿using BadgeBoard.Api.Modules.BadgeAccount.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class UserDto
	{
		public UserAccountDto Account { get; set; }

		public string Username { get; set; }
		public string? AvatarUrl { get; set; }
		public string? Title { get; set; }

		public bool IsLocked { get; set; }
		public bool IsAdmin { get; set; }

		public UserPreferenceDto Preference { get; set; }
		public UserInfoDto Info { get; set; }
	}

	public class UserPreferenceDto
	{
		public bool IsDefaultPublic { get; set; }
	}

	public class UserInfoDto
	{
		public string? Motto { get; set; }
		public string? Birthday { get; set; }
		public int Sex { get; set; } = UserSex.Unknown;
	}
}