using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class RegisterService : BadgeService, IRegisterService
	{
		public RegisterService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public async Task<ApiResponse> SendVerificationCode(VerificationCodeDto dto)
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
			if (emailType == EmailTypes.Register) {
				new EmailUtil(_provider).SendRegistrationEmailAsync("123@123", "123");
			}

			return new GoodResponse(new GoodDto());
		}
	}
}
