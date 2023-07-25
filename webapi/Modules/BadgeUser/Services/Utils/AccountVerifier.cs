// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Text.RegularExpressions;

namespace BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

public static class AccountVerifier
{
	public static bool VerifyPassword(string? password)
	{
		//return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"^(?=.*\d)(?=(.*\W){1})(?=.*[a-zA-Z])(?!.*\s).{6,16}$", RegexOptions.IgnoreCase);
		return !string.IsNullOrEmpty(password) &&
		       Regex.IsMatch(password, @"[a-zA-Z0-9_-]{6,16}$", RegexOptions.IgnoreCase);
	}

	public static bool VerifyEmail(string? email)
	{
		return string.IsNullOrEmpty(email) || Regex.IsMatch(email, @"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$",
			RegexOptions.IgnoreCase);
	}

	public static bool VerifyUsername(string? username)
	{
		return !string.IsNullOrEmpty(username) &&
		       Regex.IsMatch(username, @"^[ a-zA-Z0-9_-]{3,20}$", RegexOptions.IgnoreCase);
	}

	public static bool VerifyAccount(string? username, string? password, string? email = null)
	{
		return VerifyUsername(username) && VerifyPassword(password) && VerifyEmail(email);
	}

	public static bool VerifyAccountLoose(string? username, string? password, string? email = null)
	{
		return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
	}
}