using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class VerificationCodeDto
	{
		public string Email { get; set; }
		public string Type { get; set; }
	}

	public class VerificationCodeSuccessDto : GoodDto
	{
		public VerificationCodeSuccessDto() : base("Verification code sent") { }
	}

	public class VerificationCodeEmailErrorDto : BadRequestDto
	{
		public VerificationCodeEmailErrorDto() : base("Email format error") { }
	}

	public class VerificationCodeTypeErrorDto : BadRequestDto
	{
		public VerificationCodeTypeErrorDto() : base("Verification code type error") { }
	}
}
