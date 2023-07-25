// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.Sample.Models;

namespace BadgeBoard.Api.Modules.Sample;

public class WeatherRepository : Repository<Weather>
{
	public WeatherRepository(BadgeContext context) : base(context)
	{
	}
}