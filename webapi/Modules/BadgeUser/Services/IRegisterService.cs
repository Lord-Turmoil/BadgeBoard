using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IRegisterService
	{
		public ApiResponse SendCode(VerificationCodeDto dto);

		public Task<ApiResponse> Register(RegisterDto dto);
	}
}
