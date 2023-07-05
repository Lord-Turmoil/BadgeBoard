using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IRegisterService
	{
		public Task<ApiResponse> SendVerificationCode(VerificationCodeDto dto);
	}
}
