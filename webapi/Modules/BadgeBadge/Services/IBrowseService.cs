// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services;

/// <summary>
///     Functions without id parameter get badges of other users. While
///     those with id get badges of user him/her self, where id is the
///     initiator's user id, or, a user with administrative permission.
/// </summary>
public interface IBrowseService : IService
{
    Task<ApiResponse> GetBadge(int badgeId);
    Task<ApiResponse> GetBadge(int id, int badgeId);

    Task<ApiResponse> GetBadgesOfUser(BrowseAllBadgeDto dto);
    Task<ApiResponse> GetBadgesOfUser(int id, BrowseAllBadgeDto dto);

    Task<ApiResponse> GetBadgesOfCategory(BrowseCategoryBadgeDto dto);
    Task<ApiResponse> GetBadgesOfCategory(int id, BrowseCategoryBadgeDto dto);
}