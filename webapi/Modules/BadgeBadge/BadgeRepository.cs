using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeBadge
{
	public class BadgeRepository : Repository<Badge>
	{
		public BadgeRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}

	public class QuestionPayloadRepository : Repository<QuestionPayload>
	{
		public QuestionPayloadRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}

	public class MemoryPayloadRepository : Repository<MemoryPayload>
	{
		public MemoryPayloadRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}

	public class CategoryRepository : Repository<Category>
	{
		public CategoryRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}
}
