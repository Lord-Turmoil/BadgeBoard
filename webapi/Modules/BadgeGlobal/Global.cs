namespace BadgeBoard.Api.Modules.BadgeGlobal
{
	public class Global
	{
		private Global() { }

		public static IConfiguration Configuration { get; set; }
		public static IServiceCollection Services { get; set; }
	}
}
