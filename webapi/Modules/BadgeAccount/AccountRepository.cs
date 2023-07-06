using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount
{
	public class AccountRepository : Repository<UserAccount>
	{
		public AccountRepository(BadgeContext context) : base(context)
		{
		}
	}

	public class EmailRecordRepository : Repository<EmailRecord>
	{
		public EmailRecordRepository(BadgeContext context) : base(context)
		{
		}
	}
}
