// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

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
				.AddCustomRepository<Category, CategoryRepository>()
				.AddCustomRepository<CategoryOption, CategoryOptionRepository>();

			return services;
		}
	}
}
