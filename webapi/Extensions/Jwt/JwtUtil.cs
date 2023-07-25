// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BadgeBoard.Api.Extensions.Jwt;

public static class JwtUtil
{
    public static string CreateToken(IOptions<JwtOptions> options, string value)
    {
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Name, value)
        };

        JwtOptions opt = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Key));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            opt.Issuer,
            opt.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(opt.DurationInMinutes),
            signingCredentials: credential);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public static string? GetValueFromBearerToken(string token)
    {
        JwtSecurityToken? jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        if (jwt == null) return null;

        using IEnumerator<Claim> it = jwt.Claims.GetEnumerator();
        while (it.MoveNext())
        {
            Claim? claim = it.Current;
            if (claim is not { Type: JwtRegisteredClaimNames.Name }) continue;

            return claim.Value;
        }

        return null;
    }
}