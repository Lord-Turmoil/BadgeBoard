// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
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

[Route("api/badge/question")]
[ApiController]
public class QuestionBadgeController : BaseController<QuestionBadgeController>
{
    private readonly IBadgeService _service;


    public QuestionBadgeController(ILogger<QuestionBadgeController> logger, IBadgeService service) : base(logger)
    {
        _service = service;
    }


    [HttpPost]
    [Route("anonymous")]
    public async Task<ApiResponse> AddQuestionBadge([FromBody] AddQuestionBadgeDto dto)
    {
        try
        {
            return await _service.AddQuestionBadge(0, dto);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpPost]
    [Route("identified")]
    [Authorize]
    public async Task<ApiResponse> AddQuestionBadge(
        [FromHeader] string authorization,
        [FromBody] AddQuestionBadgeDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.AddQuestionBadge(id, dto);
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


    [HttpPost]
    [Route("update")]
    [Authorize]
    public async Task<ApiResponse> UpdateQuestionBadge(
        [FromHeader] string authorization,
        [FromBody] UpdateQuestionBadgeDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdateQuestionBadge(id, dto);
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