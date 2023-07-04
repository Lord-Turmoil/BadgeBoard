using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
    }
}
