// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Services;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeBadge.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController : BaseController<CategoryController>
{
    private readonly ICategoryService _service;


    public CategoryController(ILogger<CategoryController> logger, ICategoryService service) : base(logger)
    {
        _service = service;
    }


    [HttpGet]
    [Route("exists")]
    public async Task<ApiResponse> Exists([FromQuery] int id, [FromQuery] string name)
    {
        try
        {
            return await _service.Exists(id, name);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpPost]
    [Route("add")]
    [Authorize]
    public async Task<ApiResponse> AddCategory(
        [FromHeader] string authorization,
        [FromBody] AddCategoryDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.AddCategory(id, dto);
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
    [Route("delete")]
    [Authorize]
    public async Task<ApiResponse> DeleteCategory([FromHeader] string authorization, [FromBody] DeleteCategoryDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.DeleteCategory(id, dto);
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
    public async Task<ApiResponse> UpdateCategory([FromHeader] string authorization, [FromBody] UpdateCategoryDto nullableDto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.UpdateCategory(id, nullableDto);
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
    [Route("merge")]
    public async Task<ApiResponse> MergeCategory([FromHeader] string authorization, [FromBody] MergeCategoryDto dto)
    {
        try
        {
            var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.MergeCategory(id, dto);
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
    [Route("get")]
    public async Task<ApiResponse> GetCategory([FromQuery] int id)
    {
        try
        {
            return await _service.GetCategories(id);
        }
        catch (Exception ex)
        {
            return new GoodResponse(new UnexpectedErrorDto(ex));
        }
    }


    [HttpGet]
    [Route("current")]
    [Authorize]
    public async Task<ApiResponse> GetCategory([FromHeader] string authorization, [FromQuery] int id)
    {
        try
        {
            var initiatorId = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
            return await _service.GetCategories(initiatorId, id);
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