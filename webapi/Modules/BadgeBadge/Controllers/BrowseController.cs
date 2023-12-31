﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Services;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeBadge.Controllers;

[ApiController]
[Route("api/badge/browse")]
public class BrowseController : BaseController<BrowseController>
{
    private readonly IBrowseService _service;


    public BrowseController(ILogger<BrowseController> logger, IBrowseService service) : base(logger)
    {
        _service = service;
    }


    [HttpGet]
    [Route("anonymous/badge/{badgeId}")]
    public async Task<ApiResponse> GetBadge(int badgeId)
    {
        try
        {
            return await _service.GetBadge(badgeId);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpGet]
    [Route("identified/badge/{badgeId}")]
    [Authorize]
    public async Task<ApiResponse> GetBadge(int badgeId, [FromHeader] string authorization)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.GetBadge(id, badgeId);
        }
        catch (BadJwtTokenException)
        {
            return new UnauthorizedResponse(new BadUserJwtDto());
        }
    }


    [HttpGet]
    [Route("anonymous/user/{userId}")]
    public async Task<ApiResponse> GetBadgesOfUser(int userId, [FromQuery] string timestamp)
    {
        try
        {
            return await _service.GetBadgesOfUser(new BrowseAllBadgeDto {
                UserId = userId,
                Timestamp = timestamp
            });
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpGet]
    [Route("identified/user/{userId}")]
    [Authorize]
    public async Task<ApiResponse> GetBadgesOfUser(
        int userId,
        [FromHeader] string authorization,
        [FromQuery] string timestamp)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.GetBadgesOfUser(id, new BrowseAllBadgeDto {
                UserId = userId,
                Timestamp = timestamp
            });
        }
        catch (BadJwtTokenException)
        {
            return new UnauthorizedResponse(new BadUserJwtDto());
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpGet]
    [Route("anonymous/category/{categoryId}")]
    public async Task<ApiResponse> GetBadgesOfCategory(
        int categoryId,
        [FromQuery] string? timestamp)
    {
        try
        {
            return await _service.GetBadgesOfCategory(new BrowseCategoryBadgeDto {
                CategoryId = categoryId,
                Timestamp = timestamp
            });
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpGet]
    [Route("identified/category/{categoryId}")]
    public async Task<ApiResponse> GetBadgesOfCategory(
        int categoryId,
        [FromHeader] string authorization,
        [FromQuery] string? timestamp)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.GetBadgesOfCategory(id, new BrowseCategoryBadgeDto {
                CategoryId = categoryId,
                Timestamp = timestamp
            });
        }
        catch (BadJwtTokenException)
        {
            return new UnauthorizedResponse(new BadUserJwtDto());
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }
}