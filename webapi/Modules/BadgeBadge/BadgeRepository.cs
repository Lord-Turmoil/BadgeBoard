// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeBadge
{
	public class BadgeRepository : Repository<Badge>
	{
		public BadgeRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class QuestionPayloadRepository : Repository<QuestionPayload>
	{
		public QuestionPayloadRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class MemoryPayloadRepository : Repository<MemoryPayload>
	{
		public MemoryPayloadRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class CategoryRepository : Repository<Category>
	{
		public CategoryRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class CategoryOptionRepository : Repository<CategoryOption>
	{
		public CategoryOptionRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}

	public class UnreadRecordRepository : Repository<UnreadRecord>
	{
		public UnreadRecordRepository(BadgeContext dbContext) : base(dbContext)
		{
		}
	}
}
