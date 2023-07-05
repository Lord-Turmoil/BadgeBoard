﻿using BadgeBoard.Api.Extensions.Module;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : BadgeController
	{
		public LoginController(IServiceProvider provider) : base(provider)
		{
		}
	}
}