// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Globalization;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;

public static class BadgeDtoUtil
{
    private const string TimestampFormat = "yyyy-MM-dd-HH:mm:ss.ffffff";


    public static async Task<BadgeDto> GetQuestionBadgeDtoAsync(
        IUnitOfWork unitOfWork, IMapper mapper, QuestionBadgePack pack)
    {
        QuestionBadgeDto? badgeDto = mapper.Map<Badge, QuestionBadgeDto>(pack.Badge);
        badgeDto.Payload = mapper.Map<QuestionPayload, QuestionPayloadDto>(pack.Payload);

        await _ConstructBadgeDto(
            pack.Badge,
            badgeDto,
            unitOfWork.GetRepository<User>(),
            unitOfWork.GetRepository<Category>(),
            mapper);

        return badgeDto;
    }


    public static async Task<BadgeDto> GetMemoryBadgeDtoAsync(
        IUnitOfWork unitOfWork, IMapper mapper, MemoryBadgePack pack)
    {
        MemoryBadgeDto? badgeDto = mapper.Map<Badge, MemoryBadgeDto>(pack.Badge);
        badgeDto.Payload = mapper.Map<MemoryPayload, MemoryPayloadDto>(pack.Payload);

        await _ConstructBadgeDto(
            pack.Badge,
            badgeDto,
            unitOfWork.GetRepository<User>(),
            unitOfWork.GetRepository<Category>(),
            mapper);

        return badgeDto;
    }


    public static async Task<BadgeDto> GetBadgeDtoAsync(
        IUnitOfWork unitOfWork, IMapper mapper, Badge badge)
    {
        IRepository<QuestionPayload> questionRepo = unitOfWork.GetRepository<QuestionPayload>();
        IRepository<MemoryPayload> memoryRepo = unitOfWork.GetRepository<MemoryPayload>();

        BadgeDto badgeDto = badge.Type switch {
            Badge.Types.Question => await _GetQuestionBadgeDtoAsync(questionRepo, mapper, badge),
            Badge.Types.Memory => await _GetMemoryBadgeDtoAsync(memoryRepo, mapper, badge),
            _ => throw new Exception($"Invalid badge type {badge.Type}")
        };

        await _ConstructBadgeDto(
            badge,
            badgeDto,
            unitOfWork.GetRepository<User>(),
            unitOfWork.GetRepository<Category>(),
            mapper);

        return badgeDto;
    }


    public static async Task<IList<BadgeDto>> GetBadgeDtosAsync(
        IUnitOfWork unitOfWork, IMapper mapper, IEnumerable<Badge> badges)
    {
        var badgeDtoList = new List<BadgeDto>();

        IRepository<User> userRepo = unitOfWork.GetRepository<User>();
        IRepository<QuestionPayload> questionRepo = unitOfWork.GetRepository<QuestionPayload>();
        IRepository<MemoryPayload> memoryRepo = unitOfWork.GetRepository<MemoryPayload>();
        IRepository<Category> categoryRepo = unitOfWork.GetRepository<Category>();

        foreach (Badge badge in badges)
        {
            BadgeDto badgeDto = badge.Type switch {
                Badge.Types.Question => await _GetQuestionBadgeDtoAsync(questionRepo, mapper, badge),
                Badge.Types.Memory => await _GetMemoryBadgeDtoAsync(memoryRepo, mapper, badge),
                _ => throw new Exception($"Invalid badge type {badge.Type}")
            };

            await _ConstructBadgeDto(badge, badgeDto, userRepo, categoryRepo, mapper);

            badgeDtoList.Add(badgeDto);
        }

        return badgeDtoList;
    }


    private static async Task _ConstructBadgeDto(
        Badge badge,
        BadgeDto dto,
        IRepository<User> userRepo,
        IRepository<Category> categoryRepo,
        IMapper mapper)
    {
        User user;
        // sender
        if (dto.Sender != 0)
        {
            // Exception in GetAsync will be caught outside
            user = await User.GetAsync(userRepo, dto.Sender);
            dto.SrcUser = mapper.Map<User, UserBriefDto>(user);
        }
        else
        {
            dto.SrcUser = null;
        }

        // receiver
        user = await User.GetAsync(userRepo, dto.Receiver);
        dto.DstUser = mapper.Map<User, UserBriefDto>(user);

        // category
        if (badge.CategoryId != null)
        {
            Category category = await Category.GetAsync(categoryRepo, badge.CategoryId);
            dto.Category = mapper.Map<Category, CategoryBriefDto>(category);
        }
        else
        {
            dto.Category = null;
        }

        // timestamp
        dto.Timestamp = FormatTimestamp(dto.CreatedTime);
    }


    private static async Task<QuestionBadgeDto> _GetQuestionBadgeDtoAsync(
        IRepository<QuestionPayload> repo, IMapper mapper, Badge badge)
    {
        QuestionBadgeDto? badgeDto = mapper.Map<Badge, QuestionBadgeDto>(badge);

        QuestionPayload? payload = await QuestionPayload.FindAsync(repo, badge.PayloadId);
        if (payload == null)
        {
            throw new MissingReferenceException($"Missing QuestionPayload {badge.PayloadId} in {badge.Id}");
        }

        badgeDto.Payload = mapper.Map<QuestionPayload, QuestionPayloadDto>(payload);

        return badgeDto;
    }


    private static async Task<MemoryBadgeDto> _GetMemoryBadgeDtoAsync(
        IRepository<MemoryPayload> repo, IMapper mapper, Badge badge)
    {
        MemoryBadgeDto? badgeDto = mapper.Map<Badge, MemoryBadgeDto>(badge);

        MemoryPayload? payload = await MemoryPayload.FindAsync(repo, badge.PayloadId);
        if (payload == null)
        {
            throw new MissingReferenceException($"Missing QuestionPayload {badge.PayloadId} in {badge.Id}");
        }

        badgeDto.Payload = mapper.Map<MemoryPayload, MemoryPayloadDto>(payload);

        return badgeDto;
    }


    public static string FormatTimestamp(DateTime timestamp)
    {
        return timestamp.ToString(TimestampFormat, CultureInfo.InvariantCulture);
    }


    public static DateTime ParseTimestamp(string timestamp)
    {
        try
        {
            return DateTime.ParseExact(timestamp, TimestampFormat, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            throw new FormatException("Bad time format", ex);
        }
    }
}