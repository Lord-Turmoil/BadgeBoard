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
