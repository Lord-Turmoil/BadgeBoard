using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface ILoginService
	{
		public Task<ApiResponse> Login(LoginDto dto);

		public Task<TokenResponseData> GetToken(TokenDto dto);
	}
}
