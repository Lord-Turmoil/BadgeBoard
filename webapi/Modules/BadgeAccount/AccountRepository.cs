// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;

namespace BadgeBoard.Api.Modules.BadgeAccount;

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