namespace BadgeBoard.Api.Extensions.Email
{
	public class EmailOptions
	{
		public const string EmailSection = "EmailOptions";

		public string Email { get; set; } = string.Empty;
		public string DisplayName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Host { get; set; } = string.Empty;
		public int Port { get; set; }
	}
}
