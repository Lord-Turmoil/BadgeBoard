// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos;

public class UpdateUserPreferenceDto : IApiRequestDto
{
	public bool? IsDefaultPublic { get; set; }

	public bool Verify()
	{
		return true;
	}

	public IApiRequestDto Format()
	{
		return this;
	}
}

public class UpdateUserInfoDto : IApiRequestDto
{
	public string? Motto { get; set; }
	public string? Birthday { get; set; }
	public int? Sex { get; set; }

	public bool Verify()
	{
		if (Motto is { Length: > Globals.MaxMottoLength }) return false;
		if (Sex != null && !UserSex.IsValid((int)Sex)) return false;
		return true;
	}

	public IApiRequestDto Format()
	{
		Motto = Motto?.Trim();
		Birthday = Birthday?.Trim();
		return this;
	}
}

public class UpdateUsernameDto : IApiRequestDto
{
	public string? Username { get; set; }

	public bool Verify()
	{
		return true;
	}

	public IApiRequestDto Format()
	{
		Username = Username?.Trim();
		return this;
	}
}

public class UpdateAvatarDto : IApiRequestDto
{
	public string Extension { get; set; }
	public string Data { get; set; } // base 64 string

	public bool Verify()
	{
		return true;
	}

	public IApiRequestDto Format()
	{
		return this;
	}
}