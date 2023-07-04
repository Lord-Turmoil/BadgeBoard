using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules
{
    public class BadgeContext : DbContext
    {
        // Sample Module
        // public DbSet<Weather> Weathers { get; set; }

        // Account Module
        public DbSet<Account> Accounts { get; set; }
        public DbSet<EmailRecord> EmailRecords { get; set; }

        // User Module

        public BadgeContext(DbContextOptions<BadgeContext> options) : base(options)
        {
        }
    }
}
