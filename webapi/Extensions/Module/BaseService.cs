using AutoMapper;
using BadgeBoard.Api.Extensions.UnitOfWork;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BaseService
	{
		protected readonly IServiceProvider _provider;
		protected readonly IUnitOfWork _unitOfWork;
		protected readonly IMapper _mapper;

		protected BaseService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_provider = provider;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
	}
}
