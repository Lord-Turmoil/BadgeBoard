namespace BadgeBoard.Api.Extensions.Jwt
{
	public class JwtOptions
	{
		public const string JwtSection = "JwtOptions";

		public string Key { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public double DurationInMinutes { get; set; }
	}
}
