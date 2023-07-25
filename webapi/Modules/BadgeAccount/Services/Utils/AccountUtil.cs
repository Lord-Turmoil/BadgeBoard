// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Security.Cryptography;
using System.Text;

namespace BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;

public static class AccountUtil
{
	private const int MinUserId = 1000000000;
	private const int MaxUserId = 2000000000;

	public static int GenerateAccountId()
	{
		return (int)Random.Shared.NextInt64(MinUserId, MaxUserId + 1);
	}

	public static void CreatePasswordHash(string password, out byte[] salt, out byte[] hash)
	{
		using var hmac = new HMACSHA256();
		salt = hmac.Key;
		hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
	}

	public static bool VerifyPasswordHash(string password, byte[] salt, byte[] hash)
	{
		using var hmac = new HMACSHA256(salt);
		var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

		return computeHash.SequenceEqual(hash);
	}
}