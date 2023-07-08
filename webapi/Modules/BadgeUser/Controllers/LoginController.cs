using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class LoginController : BaseController<LoginController>
	{
		private readonly ILoginService _service;

		public LoginController(ILogger<LoginController> logger, ILoginService service) : base(logger)
		{
			_service = service;
		}

		[HttpPost]
		[Route("login")]
		public async Task<ApiResponse> Login([FromBody] LoginDto dto)
		{
			return await _service.Login(dto);
		}

		[HttpPost]
		[Route("token/get")]
		public async Task<ApiResponse> GetToken([FromBody] TokenDto dto)
		{
			var data = await _service.GetToken(dto);
			if (!data.IsAuthenticated) {
				return new ApiResponse(data.Status, new GetTokenFailedDto(data.Message));
			}

			SetRefreshTokenInCookie(data.RefreshToken);

			return new GoodResponse(new GoodWithDataDto(data));
		}

		[HttpPost]
		[Route("token/refresh")]
		public async Task<ApiResponse> RefreshToken()
		{
			var refreshToken = Request.Cookies[TokenUtil.RefreshTokenCookiesName];
			if (refreshToken == null) {
				return new BadRequestResponse(new BadRequestDto("Missing essential cookies"));
			}

			var data = await _service.RefreshToken(refreshToken);
			if (!data.IsAuthenticated) {
				return new ApiResponse(data.Status, new RefreshTokenFailedDto(data.Message));
			}

			SetRefreshTokenInCookie(data.RefreshToken);

			return new GoodResponse(new GoodWithDataDto(data));
		}

		private void SetRefreshTokenInCookie(string token)
		{
			var options = TokenUtil.GetRefreshTokenCookieOptions();
			AddCookie(TokenUtil.RefreshTokenCookiesName, token, options);
		}
		
		private void AddCookie(string name, string value, CookieOptions options)
		{
			Response.Cookies.Append(name, value, options);
		}
	}
}
