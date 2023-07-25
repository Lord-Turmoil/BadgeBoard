// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Modules.Sample.Models;

namespace BadgeBoard.Api.Modules.Sample;

public class SampleModule : BaseModule
{
	public override IServiceCollection RegisterModule(IServiceCollection services)
	{
		services.AddCustomRepository<Weather, WeatherRepository>();

		return services;
	}
}