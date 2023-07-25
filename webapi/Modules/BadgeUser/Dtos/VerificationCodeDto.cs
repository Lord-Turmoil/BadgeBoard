// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos;

public class VerificationCodeDto : IApiRequestDto
{
	public string Email { get; set; }
	public string Type { get; set; }

	public bool Verify()
	{
		return !(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Type));
	}

	public IApiRequestDto Format()
	{
		return this;
	}
}

public class VerificationCodeSuccessDto : GoodDto
{
	public VerificationCodeSuccessDto() : base("Verification code sent")
	{
	}
}

public class VerificationCodeEmailErrorDto : BadRequestDto
{
	public VerificationCodeEmailErrorDto() : base("Email format error")
	{
	}
}

public class VerificationCodeTypeErrorDto : BadRequestDto
{
	public VerificationCodeTypeErrorDto() : base("Verification code type error")
	{
	}
}