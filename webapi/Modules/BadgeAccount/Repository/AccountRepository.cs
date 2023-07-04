using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount.Repository
{
    public class AccountRepository : Repository<Account>
	{
		public AccountRepository(AccountContext context) : base(context)
		{
		}
	}
}
