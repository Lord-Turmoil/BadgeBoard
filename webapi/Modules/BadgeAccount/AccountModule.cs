// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount;

public class AccountModule : BaseModule
{
	public override IServiceCollection RegisterModule(IServiceCollection services)
	{
		services.AddCustomRepository<UserAccount, AccountRepository>()
			.AddCustomRepository<EmailRecord, EmailRecordRepository>();

		return services;
	}
}