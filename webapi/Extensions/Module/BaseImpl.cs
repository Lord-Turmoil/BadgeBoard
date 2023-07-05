using AutoMapper;
using BadgeBoard.Api.Extensions.UnitOfWork;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BaseImpl
	{
		protected readonly IServiceProvider _provider;
		protected readonly IUnitOfWork _unitOfWork;
		protected readonly IMapper _mapper;

		public BaseImpl(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_provider = provider;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
	}
}
