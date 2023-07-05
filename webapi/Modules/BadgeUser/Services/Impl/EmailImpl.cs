﻿using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

namespace BadgeBoard.Api.Modules.BadgeUser.Services.Impl
{
	public class EmailImpl : BaseImpl
	{
		public EmailImpl(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public ApiResponse SendVerificationCode(VerificationCodeDto dto)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = UserUtil.GetUserByEmail(repo, dto.Email);
			if (user != null) {
				return new GoodResponse(new UserAlreadyExistsDto());
			}

			return new GoodResponse(new GoodDto("Verification code sent"));
		}

		public ApiResponse SendRetrievalCode(VerificationCodeDto dto)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = UserUtil.GetUserByEmail(repo, dto.Email);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			return new GoodResponse(new GoodDto("Retrieval code sent"));
		}
	}
}