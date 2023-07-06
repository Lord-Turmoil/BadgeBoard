using BadgeBoard.Api.Extensions.Module;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount
{
    public class AccountModule : BaseModule
	{
		public override IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<UserAccount, AccountRepository>()
				.AddCustomRepository<EmailRecord, EmailRecordRepository>();

			return services;
		}
	}
}
