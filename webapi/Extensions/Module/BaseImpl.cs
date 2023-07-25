// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BaseImpl
	{
		protected readonly IServiceProvider _provider;
		protected readonly IUnitOfWork _unitOfWork;
		protected readonly IMapper _mapper;

		protected BaseImpl(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_provider = provider;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
	}
}
