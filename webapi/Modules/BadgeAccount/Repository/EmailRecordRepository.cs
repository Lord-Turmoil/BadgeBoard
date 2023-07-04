using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount.Repository
{
    public class EmailRecordRepository : Repository<EmailRecord>
	{
		public EmailRecordRepository(AccountContext context) : base(context)
		{
		}
	}
}
