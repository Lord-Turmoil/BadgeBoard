// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Security.Cryptography;
using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;

public static class TokenUtil
{
    public const int RefreshTokenExpire = 14;
    public const string RefreshTokenCookiesName = "refreshToken";


    public static RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = new RNGCryptoServiceProvider())
        {
            generator.GetBytes(randomNumber);
        }

        return new RefreshToken {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(RefreshTokenExpire),
            Created = DateTime.UtcNow
        };
    }


    public static CookieOptions GetRefreshTokenCookieOptions()
    {
        return new CookieOptions {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(RefreshTokenExpire)
        };
    }


    public static int? TryGetUserIdFromJwtBearerToken(string token)
    {
        if (token.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase)) token = token[7..];
        var value = JwtUtil.GetValueFromBearerToken(token);
        if (value == null) return null;

        return int.TryParse(value, out var id) ? id : null;
    }


    public static int GetUserIdFromJwtBearerToken(string token)
    {
        return TryGetUserIdFromJwtBearerToken(token) ?? throw new ArgumentException(token);
    }
}