﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers;

[Route("api/auth")]
[ApiController]
public class RegisterController : BaseController<RegisterController>
{
    private readonly IRegisterService _service;


    public RegisterController(ILogger<RegisterController> logger, IRegisterService service) : base(logger)
    {
        _service = service;
    }


    [HttpPost]
    [Route("code")]
    public ApiResponse SendCode([FromBody] VerificationCodeDto dto)
    {
        try
        {
            return _service.SendCode(dto);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpPost]
    [Route("register")]
    public async Task<ApiResponse> Register([FromBody] RegisterDto dto)
    {
        try
        {
            return await _service.Register(dto);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpPost]
    [Route("cancel")]
    public async Task<ApiResponse> Cancel([FromBody] CancelDto dto)
    {
        try
        {
            return await _service.Cancel(dto);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }
}