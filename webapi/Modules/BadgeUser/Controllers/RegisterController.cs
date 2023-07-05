using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : BadgeController
	{
		public RegisterController(IServiceProvider provider) : base(provider)
		{
		}

		[HttpPost]
		[Route("code")]
		public async Task<ApiResponse> SendVerificationCode([FromBody] VerificationCodeDto dto)
		{
			if (!AccountVerifier.VerifyEmail(dto.Email)) {
				return new BadRequestResponse(new VerificationCodeEmailErrorDto());
			}

			int emailType = EmailTypes.Invalid;
			if ("register".Equals(dto.Type)) {
				emailType = EmailTypes.Register;
			} else if ("retrieve".Equals(dto.Type)) {
				emailType = EmailTypes.Retrieve;
			}
			if (emailType == EmailTypes.Invalid) {
				return new BadRequestResponse(new BadRequestDto($"Invalid type: {dto.Type ?? "null"}"));
			}

			// Intended to be asynchronous.
			new UserEmailService(_provider).SendRegistrationEmailAsync("123@123", "123");

			return new GoodResponse(new GoodDto());
		}
	}
}
