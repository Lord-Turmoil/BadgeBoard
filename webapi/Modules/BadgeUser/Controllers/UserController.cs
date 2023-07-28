// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : BaseController<UserController>
{
    private readonly IUserService _service;


    public UserController(ILogger<UserController> logger, IUserService service) : base(logger)
    {
        _service = service;
    }


    [HttpGet]
    [Route("exists")]
    public async Task<ApiResponse> Exists([FromQuery] string type, [FromQuery] string value)
    {
        try
        {
            return await _service.Exists(type, value);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpPost]
    [Route("preference")]
    [Authorize]
    public async Task<ApiResponse> UpdatePreference([FromHeader] string authorization,
        [FromBody] UpdateUserPreferenceDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdatePreference(id, dto);
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
    [Route("info")]
    [Authorize]
    public async Task<ApiResponse> UpdateInfo([FromHeader] string authorization, [FromBody] UpdateUserInfoDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdateInfo(id, dto);
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
    [Route("username")]
    [Authorize]
    public async Task<ApiResponse> UpdateUsername([FromHeader] string authorization, [FromBody] UpdateUsernameDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdateUsername(id, dto);
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
    [Route("avatar")]
    [Authorize]
    public async Task<ApiResponse> UpdateAvatar([FromHeader] string authorization, [FromBody] UpdateAvatarDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdateAvatar(id, dto);
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
    [Route("user")]
    public async Task<ApiResponse> GetUser([FromQuery] int id)
    {
        try
        {
            return await _service.GetUser(id);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpGet]
    [Route("current")]
    [Authorize]
    public async Task<ApiResponse> GetCurrentUser([FromHeader] string authorization)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.GetCurrentUser(id);
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