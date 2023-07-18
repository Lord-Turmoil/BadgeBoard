namespace BadgeBoard.Api.Extensions.Cors
{
	public class CorsOptions
	{
		public const string CorsSection = "CorsOptions";
		public const string CorsPolicyName = "DefaultPolicy";

		public bool Enable { get; set; } = false;
		public List<string> Origins { get; set; } = new List<string>();
	}
}
