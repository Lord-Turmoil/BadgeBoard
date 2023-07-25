// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Impl;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

namespace BadgeBoard.Api.Modules.BadgeUser.Services;

public class RegisterService : BaseService, IRegisterService
{
	public RegisterService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider,
		unitOfWork, mapper)
	{
	}

	public ApiResponse SendCode(VerificationCodeDto dto)
	{
		if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());
		if (!AccountVerifier.VerifyEmail(dto.Email)) return new BadRequestResponse(new VerificationCodeEmailErrorDto());

		var emailType = dto.Type switch {
			"register" => EmailTypes.Register,
			"retrieve" => EmailTypes.Retrieve,
			_ => EmailTypes.Invalid
		};
		if (emailType == EmailTypes.Invalid)
			return new BadRequestResponse(new BadRequestDto($"Invalid type: {dto.Type}"));

		// Intended to be asynchronous.
		var impl = new EmailImpl(_provider, _unitOfWork, _mapper);
		switch (emailType) {
			case EmailTypes.Register:
				impl.SendVerificationCode(dto);
				break;
			case EmailTypes.Retrieve:
				impl.SendRetrievalCode(dto);
				break;
		}

		return new GoodResponse(new GoodDto());
	}

	public async Task<ApiResponse> Register(RegisterDto dto)
	{
		if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());

		var repo = _unitOfWork.GetRepository<User>();
		if (await UserUtil.HasUserByUsernameAsync(repo, dto.Username))
			return new GoodResponse(new UserAlreadyExistsDto());

		try {
			await UserUtil.CreateUserAsync(_unitOfWork, dto);
			await _unitOfWork.SaveChangesAsync();
		} catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			return new InternalServerErrorResponse(new InternalServerErrorDto("Failed to save user data"));
		}

		return new GoodResponse(new GoodDto("Welcome, my friend"));
	}


	public async Task<ApiResponse> Cancel(CancelDto dto)
	{
		if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());

		var repo = _unitOfWork.GetRepository<User>();
		foreach (var username in dto.Users.Where(username => !string.IsNullOrEmpty(username))) {
			var user = await UserUtil.FindUserByUsernameAsync(repo, username);
			if (user != null) UserUtil.EraseUser(_unitOfWork, user);
		}

		try {
			await _unitOfWork.SaveChangesAsync();
		} catch (Exception ex) {
			return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
		}

		return new GoodResponse(new GoodDto("Users canceled"));
	}
}