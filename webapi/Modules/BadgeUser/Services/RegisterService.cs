using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services.Impl;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class RegisterService : BaseService, IRegisterService
	{
		public RegisterService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public ApiResponse SendCode(VerificationCodeDto dto)
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
			var impl = new EmailImpl(_provider, _unitOfWork, _mapper);
			if (emailType == EmailTypes.Register) {
				impl.SendVerificationCode(dto);
			} else if (emailType == EmailTypes.Retrieve) {
				impl.SendRetrievalCode(dto);
			}

			return new GoodResponse(new GoodDto());
		}
	}
}
