﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils
{
    public static class BadgeUtil
	{
		public static async Task<QuestionBadgePack> AddQuestionBadgeAsync(
			IUnitOfWork unitOfWork, AddQuestionBadgeDto dto,
			User? sender, User receiver, Category? category)
		{
			var payload = await QuestionPayload.CreateAsync(unitOfWork.GetRepository<QuestionPayload>(), dto.Question);
			var badge = await Badge.CreateAsync(unitOfWork.GetRepository<Badge>(),
				dto.Type, payload.Id, sender, receiver, category, dto.Style);

			return new QuestionBadgePack { Badge = badge, Payload = payload };
		}

		public static async Task<MemoryBadgePack> AddMemoryBadgeAsync(
			IUnitOfWork unitOfWork, AddMemoryBadgeDto dto,
			User? sender, User receiver, Category? category)
		{
			var payload = await MemoryPayload.CreateAsync(unitOfWork.GetRepository<MemoryPayload>(), dto.Memory);
			var badge = await Badge.CreateAsync(unitOfWork.GetRepository<Badge>(),
				dto.Type, payload.Id, sender, receiver, category, dto.Style);

			return new MemoryBadgePack { Badge = badge, Payload = payload };
		}

		public static async Task<BadgeDto> GetQuestionBadgeDtoAsync(
			IUnitOfWork unitOfWork, IMapper mapper, QuestionBadgePack pack)
		{
			var badgeDto = mapper.Map<Badge, QuestionBadgeDto>(pack.Badge);
			var userRepo = unitOfWork.GetRepository<User>();
			User? user;
			if (badgeDto.Sender != 0) {
				user = await User.FindAsync(userRepo, badgeDto.Sender);
				if (user == null) {
					throw new MissingReferenceException("Sender in badge");
				}

				badgeDto.SrcUser = mapper.Map<User, UserBriefDto>(user);
			} else {
				badgeDto.SrcUser = null;
			}

			user = await User.FindAsync(userRepo, badgeDto.Receiver);
			if (user == null) {
				throw new MissingReferenceException("Receiver in badge");
			}

			badgeDto.DstUser = mapper.Map<User, UserBriefDto>(user);

			return badgeDto;
		}
	}
}
