using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules
{
	public class BadgeContext : DbContext
	{
		// Sample Module
		// public DbSet<Weather> Weathers { get; set; }

		// Account Module
		public DbSet<UserAccount> Accounts { get; set; }
		public DbSet<EmailRecord> EmailRecords { get; set; }

		// User Module
		public DbSet<User> Users { get; set; }
		public DbSet<UserPreference> UserPreferences { get; set; }
		public DbSet<UserInfo> UserInfo { get; set; }
		public DbSet<FavoriteUser> FavoriteUsers { get; set; }

		public BadgeContext(DbContextOptions<BadgeContext> options) : base(options)
		{
		}
	}
}
