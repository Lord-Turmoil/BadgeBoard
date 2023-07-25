// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services;

/// <summary>
///     For the creation of badges. All id is the sender's id. For requests
///     that can be anonymous (only add badge), 0 means anonymous.
/// </summary>
public interface IBadgeService
{
	public Task<ApiResponse> AddQuestionBadge(int id, AddQuestionBadgeDto dto);
	public Task<ApiResponse> AddMemoryBadge(int id, AddMemoryBadgeDto dto);

	public Task<ApiResponse> DeleteBadge(int id, DeleteBadgeDto dto);

	public Task<ApiResponse> UpdateBadge(int id, UpdateBadgeDto dto);
	public Task<ApiResponse> UpdateQuestionBadge(int id, UpdateQuestionBadgeDto dto);
	public Task<ApiResponse> UpdateMemoryBadge(int id, UpdateMemoryBadgeDto dto);

	// Move badge to another category.
	public Task<ApiResponse> MoveBadge(int id, MoveBadgeDto dto);
}