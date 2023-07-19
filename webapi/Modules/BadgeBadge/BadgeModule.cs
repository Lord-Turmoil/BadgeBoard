using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Modules.BadgeBadge.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge
{
	public class BadgeModule : BaseModule
	{
		public override IServiceCollection RegisterModule(IServiceCollection services)
		{
			services.AddCustomRepository<Badge, BadgeRepository>()
				.AddCustomRepository<QuestionPayload, QuestionPayloadRepository>()
				.AddCustomRepository<MemoryPayload, MemoryPayloadRepository>()
				.AddCustomRepository<Category, CategoryRepository>();

			return services;
		}
	}
}
