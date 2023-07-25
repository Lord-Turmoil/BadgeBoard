// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Services;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeBadge.Controllers;

[Route("api/badge")]
[ApiController]
public class BadgeController : BaseController<BadgeController>
{
    private readonly IBadgeService _service;


    public BadgeController(ILogger<BadgeController> logger, IBadgeService service) : base(logger)
    {
        _service = service;
    }


    [HttpPost]
    [Route("delete")]
    [Authorize]
    public async Task<ApiResponse> DeleteBadge([FromHeader] string authorization, [FromBody] DeleteBadgeDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.DeleteBadge(id, dto);
        }
        catch (ArgumentException)
        {
            return new UnauthorizedResponse(new BadUserJwtDto());
        }
    }


    [HttpPost]
    [Route("update")]
    [Authorize]
    public async Task<ApiResponse> UpdateBadge([FromHeader] string authorization, [FromBody] UpdateBadgeDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdateBadge(id, dto);
        }
        catch (ArgumentException)
        {
            return new UnauthorizedResponse(new BadUserJwtDto());
        }
    }


    [HttpPost]
    [Route("move")]
    [Authorize]
    public async Task<ApiResponse> MoveBadge([FromHeader] string authorization, [FromBody] MoveBadgeDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.MoveBadge(id, dto);
        }
        catch (ArgumentException)
        {
            return new UnauthorizedResponse(new BadUserJwtDto());
        }
    }
}