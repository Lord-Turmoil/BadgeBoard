// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;

public static class BadgeUtil
{
    public static async Task<QuestionBadgePack> AddQuestionBadgeAsync(
        IUnitOfWork unitOfWork, AddQuestionBadgeDto dto,
        User? sender, User receiver, Category? category)
    {
        var payload = await QuestionPayload.CreateAsync(unitOfWork.GetRepository<QuestionPayload>(), dto.Question);
        // WARNING! Since payload and badge are not connected by foreign key,
        // we can only get payload id after changes are saved!!! :(
        await unitOfWork.SaveChangesAsync();

        var badge = await Badge.CreateAsync(unitOfWork.GetRepository<Badge>(),
            dto.Type, payload.Id, sender, receiver, category, dto.Style);

        return new QuestionBadgePack { Badge = badge, Payload = payload };
    }


    public static async Task<MemoryBadgePack> AddMemoryBadgeAsync(
        IUnitOfWork unitOfWork, AddMemoryBadgeDto dto,
        User? sender, User receiver, Category? category)
    {
        var payload = await MemoryPayload.CreateAsync(unitOfWork.GetRepository<MemoryPayload>(), dto.Memory);
        await unitOfWork.SaveChangesAsync();
        var badge = await Badge.CreateAsync(unitOfWork.GetRepository<Badge>(),
            dto.Type, payload.Id, sender, receiver, category, dto.Style);

        return new MemoryBadgePack { Badge = badge, Payload = payload };
    }


    private static void _EraseBadge(IUnitOfWork unitOfWork, Badge badge, UnreadRecord? unread)
    {
        if (badge.Type == Badge.Types.Question)
        {
            unitOfWork.GetRepository<QuestionPayload>().Delete(badge.PayloadId);
            if (unread != null)
            {
                unread.QuestionCount--;
            }
        }
        else
        {
            unitOfWork.GetRepository<MemoryPayload>().Delete(badge.PayloadId);
            if (unread != null)
            {
                unread.MemoryCount--;
            }
        }

        unitOfWork.GetRepository<Badge>().Delete(badge);
    }


    public static async Task<List<DeleteErrorData>> EraseBadgesAsync(
        IUnitOfWork unitOfWork, IEnumerable<int> badges, User user, bool force = false)
    {
        var badgeList = new List<Badge>();
        var errors = new List<DeleteErrorData>();
        IRepository<Badge> repo = unitOfWork.GetRepository<Badge>();

        foreach (var badgeId in badges)
        {
            Badge? badge = await Badge.FindAsync(repo, badgeId);
            if (badge == null)
            {
                errors.Add(new DeleteErrorData {
                    Id = badgeId,
                    Message = "Badge does not exist"
                });
                continue;
            }

            badgeList.Add(badge);
        }

        errors.AddRange(await EraseBadges(unitOfWork, badgeList, user, force));

        return errors;
    }


    public static async Task<List<DeleteErrorData>> EraseBadges(
        IUnitOfWork unitOfWork, IEnumerable<Badge> badges, User user, bool force = false)
    {
        var errors = new List<DeleteErrorData>();
        user.Unread ??= await UnreadRecord.GetAsync(unitOfWork.GetRepository<UnreadRecord>(), user.UnreadRecordId);

        foreach (Badge badge in badges)
        {
            if (badge.UserId == user.Id)
            {
                // delete badge of user him/her self
                _EraseBadge(unitOfWork, badge, user.Unread);
            }
            else if (user.IsAdmin)
            {
                // admin can delete other user's badge in force mode
                if (force)
                {
                    if (badge.IsChecked)
                    {
                        _EraseBadge(unitOfWork, badge, null);
                    }
                    else
                    {
                        errors.Add(new DeleteErrorData {
                            Id = badge.Id,
                            Message = "Badge not checked yet"
                        });
                    }
                }
                else
                {
                    errors.Add(new DeleteErrorData {
                        Id = badge.Id,
                        Message = "Not in force mode"
                    });
                }
            }
            else
            {
                // no permission
                errors.Add(new DeleteErrorData {
                    Id = badge.Id,
                    Message = "Permission denied"
                });
            }
        }

        return errors;
    }


    private static async Task EraseBadges(
        IUnitOfWork unitOfWork, IEnumerable<Badge> badges) { }


    // badge must be completely get (with category fully included)
    public static bool IsAccessible(Badge badge)
    {
        return badge is { IsPublic: true, Category.Option.IsPublic: true };
    }


    public static bool IsAccessible(Badge badge, User user)
    {
        if (IsAccessible(badge))
        {
            return true;
        }

        // badge is private
        return badge.UserId == user.Id || user.IsAdmin;
    }
}