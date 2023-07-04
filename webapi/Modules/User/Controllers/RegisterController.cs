using BadgeBoard.Api.Extensions.Email;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.User.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : Controller
	{
		private readonly IServiceProvider _provider;

		public RegisterController(IServiceProvider provider)
		{
			_provider = provider;
		}

		[HttpPost]
		[Route("code")]
		public async Task<BadgeResponse> SendVerificationCode([FromQuery] string? type)
		{
			int emailType = EmailTypes.Invalid;
			if ("register".Equals(type)) {
				emailType = EmailTypes.Register;
			} else if ("retrieve".Equals(type)) {
				emailType = EmailTypes.Retrieve;
			}

			if (emailType == EmailTypes.Invalid) {
				return new BadRequestResponse(new BadRequestDto($"Invalid type: {type}"));
			}

			new UserEmailService(_provider).SendRegistrationEmailAsync("123@123", "123");

			return new GoodResponse(new GoodDto());
		}
	}
}
