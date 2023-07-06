using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class VerificationCodeDto : ApiRequestDto
	{
		public string Email { get; set; }
		public string Type { get; set; }

		public override bool Verify()
		{
			return !(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Type));
		}
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
