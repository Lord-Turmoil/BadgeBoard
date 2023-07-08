using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Email;
using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;
using Microsoft.Extensions.Options;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class LoginService : BaseService, ILoginService
	{
		public LoginService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public async Task<ApiResponse> Login(LoginDto dto)
		{
			if (!dto.Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByUsernameAsync(repo, dto.Username);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			await User.IncludeAsync(_unitOfWork, user);

			if (!AccountUtil.VerifyPasswordHash(dto.Password, user.Account.Salt, user.Account.Password)) {
				return new GoodResponse(new LoginWrongPasswordDto());
			}

			var options = _provider.GetRequiredService<IOptions<JwtOptions>>();
			var jwt = JwtUtil.CreateToken(options, user.Id.ToString(), DateTime.Now.AddDays(7));
			var userDto = _mapper.Map<User, UserDto>(user);

			return new GoodResponse(new LoginSuccessDto(userDto, jwt));
		}

	}
}
