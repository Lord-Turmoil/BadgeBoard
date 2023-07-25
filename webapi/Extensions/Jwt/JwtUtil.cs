﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BadgeBoard.Api.Extensions.Jwt
{
	public static class JwtUtil
	{
		public static string CreateToken(IOptions<JwtOptions> options, string value)
		{
			List<Claim> claims = new List<Claim> {
				new Claim(JwtRegisteredClaimNames.Name, value)
			};

			var opt = options.Value;
			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(opt.Key));
			var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer: opt.Issuer,
				audience: opt.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(opt.DurationInMinutes),
				signingCredentials: credential);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public static string? GetValueFromBearerToken(string token)
		{
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
			if (jwt == null) {
				return null;
			}

			using var it = jwt.Claims.GetEnumerator();
			while (it.MoveNext()) {
				var claim = it.Current;
				if (claim is not { Type: JwtRegisteredClaimNames.Name }) {
					continue;
				}

				return claim.Value;
			}

			return null;
		}
	}
}
