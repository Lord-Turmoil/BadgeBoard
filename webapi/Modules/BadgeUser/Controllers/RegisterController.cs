using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : BadgeController
	{
		private readonly IRegisterService _service;

		public RegisterController(IRegisterService service)
		{
			_service = service;
		}

		[HttpPost]
		[Route("code")]
		public async Task<ApiResponse> SendVerificationCode([FromBody] VerificationCodeDto dto)
		{
			return await _service.SendVerificationCode(dto);
		}
	}
}
