// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils
{
	public static class BadgeDtoUtil
	{
		public static async Task<BadgeDto> GetQuestionBadgeDtoAsync(
			IUnitOfWork unitOfWork, IMapper mapper, QuestionBadgePack pack)
		{
			var badgeDto = mapper.Map<Badge, QuestionBadgeDto>(pack.Badge);
			badgeDto.Payload = mapper.Map<QuestionPayload, QuestionPayloadDto>(pack.Payload);

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

		public static async Task<BadgeDto> GetMemoryBadgeDtoAsync(
			IUnitOfWork unitOfWork, IMapper mapper, MemoryBadgePack pack)
		{
			var badgeDto = mapper.Map<Badge, MemoryBadgeDto>(pack.Badge);
			badgeDto.Payload = mapper.Map<MemoryPayload, MemoryPayloadDto>(pack.Payload);

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

		public static async Task<BadgeDto> GetBadgeDtoAsync(
			IUnitOfWork unitOfWork, IMapper mapper, Badge badge)
		{
			var userRepo = unitOfWork.GetRepository<User>();
			var questionRepo = unitOfWork.GetRepository<QuestionPayload>();
			var memoryRepo = unitOfWork.GetRepository<MemoryPayload>();

			BadgeDto badgeDto = badge.Type switch {
				Badge.Types.Question => await GetQuestionBadgeDtoAsync(questionRepo, mapper, badge),
				Badge.Types.Memory => await GetMemoryBadgeDtoAsync(memoryRepo, mapper, badge),
				_ => throw new Exception($"Invalid badge type {badge.Type}")
			};

			User? user;
			// sender
			if (badgeDto.Sender != 0) {
				user = await User.FindAsync(userRepo, badgeDto.Sender);
				if (user == null) {
					throw new MissingReferenceException($"Sender {badgeDto.Sender}");
				}
				badgeDto.SrcUser = mapper.Map<User, UserBriefDto>(user);
			} else {
				badgeDto.SrcUser = null;
			}

			// receiver
			user = await User.FindAsync(userRepo, badgeDto.Receiver);
			if (user == null) {
				throw new MissingReferenceException($"Receiver {badgeDto.Receiver}");
			}
			badgeDto.DstUser = mapper.Map<User, UserBriefDto>(user);

			return badgeDto;
		}

		public static async Task<IList<BadgeDto>> GetBadgeDtoListAsync(
			IUnitOfWork unitOfWork, IMapper mapper, IEnumerable<Badge> badges)
		{
			var badgeDtoList = new List<BadgeDto>();

			var userRepo = unitOfWork.GetRepository<User>();
			var questionRepo = unitOfWork.GetRepository<QuestionPayload>();
			var memoryRepo = unitOfWork.GetRepository<MemoryPayload>();

			foreach (var badge in badges) {
				BadgeDto badgeDto = badge.Type switch {
					Badge.Types.Question => await GetQuestionBadgeDtoAsync(questionRepo, mapper, badge),
					Badge.Types.Memory => await GetMemoryBadgeDtoAsync(memoryRepo, mapper, badge),
					_ => throw new Exception($"Invalid badge type {badge.Type}")
				};

				User? user;
				// sender
				if (badgeDto.Sender != 0) {
					user = await User.FindAsync(userRepo, badgeDto.Sender);
					if (user == null) {
						throw new MissingReferenceException($"Sender {badgeDto.Sender}");
					}

					badgeDto.SrcUser = mapper.Map<User, UserBriefDto>(user);
				} else {
					badgeDto.SrcUser = null;
				}

				// receiver
				user = await User.FindAsync(userRepo, badgeDto.Receiver);
				if (user == null) {
					throw new MissingReferenceException($"Receiver {badgeDto.Receiver}");
				}
				badgeDto.DstUser = mapper.Map<User, UserBriefDto>(user);

				badgeDtoList.Add(badgeDto);
			}

			return badgeDtoList;
		}

		private static async Task<QuestionBadgeDto> GetQuestionBadgeDtoAsync(IRepository<QuestionPayload> repo, IMapper mapper, Badge badge)
		{
			var badgeDto = mapper.Map<Badge, QuestionBadgeDto>(badge);

			var payload = await QuestionPayload.FindAsync(repo, badge.PayloadId);
			if (payload == null) {
				throw new MissingReferenceException($"Missing QuestionPayload {badge.PayloadId} in {badge.Id}");
			}

			badgeDto.Payload = mapper.Map<QuestionPayload, QuestionPayloadDto>(payload);

			return badgeDto;
		}

		private static async Task<MemoryBadgeDto> GetMemoryBadgeDtoAsync(IRepository<MemoryPayload> repo, IMapper mapper, Badge badge)
		{
			var badgeDto = mapper.Map<Badge, MemoryBadgeDto>(badge);

			var payload = await MemoryPayload.FindAsync(repo, badge.PayloadId);
			if (payload == null) {
				throw new MissingReferenceException($"Missing QuestionPayload {badge.PayloadId} in {badge.Id}");
			}

			badgeDto.Payload = mapper.Map<MemoryPayload, MemoryPayloadDto>(payload);

			return badgeDto;
		}
	}
}
