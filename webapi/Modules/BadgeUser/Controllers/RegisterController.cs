﻿using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class RegisterController : BaseController<RegisterController>
	{
		private readonly IRegisterService _service;

		public RegisterController(ILogger<RegisterController> logger, IRegisterService service) : base(logger)
		{
			_service = service;
		}

		[HttpPost]
		[Route("code")]
		public ApiResponse SendCode([FromBody] VerificationCodeDto dto)
		{
			return _service.SendCode(dto);
		}

		[HttpPost]
		[Route("register")]
		public async Task<ApiResponse> Register([FromBody] RegisterDto dto)
		{
			return await _service.Register(dto);
		}
	}
}
