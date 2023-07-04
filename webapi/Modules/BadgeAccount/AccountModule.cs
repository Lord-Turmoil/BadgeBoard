using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Repository;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BadgeBoard.Api.Modules.BadgeAccount
{
	public class AccountModule : BadgeModule
	{
		public new IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<Account, AccountRepository>()
				.AddCustomRepository<EmailRecord, EmailRecordRepository>();

			return services;
		}
	}
}
