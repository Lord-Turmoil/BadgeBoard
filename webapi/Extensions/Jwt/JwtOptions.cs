namespace BadgeBoard.Api.Extensions.Jwt
{
	public class JwtOptions
	{
		public const string JwtSection = "JwtOptions";
		public const string JwtKey = "Key";
		public const string JwtIssuer = "Issuer";
		public const string JwtAudience = "Audience";

		public string Key { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public double DurationInMinutes { get; set; }
	}
}
