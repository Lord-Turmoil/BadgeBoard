using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
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
	}
}
