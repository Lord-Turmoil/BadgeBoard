using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class EditUserService : BaseService, IEditUserService
	{
		protected EditUserService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public async Task<ApiResponse> Exists(string type, string value)
		{
			if (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(value)) {
				return new GoodResponse(new GoodWithDataDto(false));
			}

			var repo = _unitOfWork.GetRepository<User>();
			var data = type switch {
				"id" => await UserUtil.HasUserByIdAsync(repo, new Guid(value)),
				"username" => await UserUtil.HasUserByUsernameAsync(repo, value),
				_ => await UserUtil.HasUserByUsernameAsync(repo, value)
			};

			return new GoodResponse(new GoodWithDataDto(data));
		}
	}
}
