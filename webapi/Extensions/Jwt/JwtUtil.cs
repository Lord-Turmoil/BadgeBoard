using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BadgeBoard.Api.Extensions.Jwt
{
	public static class JwtUtil
	{
		public static string CreateToken(IOptions<JwtOptions> options, string value, int expire)
		{
			List<Claim> claims = new List<Claim> {
				new Claim(ClaimTypes.Name, value)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(options.Value.Token));
			var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(expire),
				signingCredentials: credential);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}