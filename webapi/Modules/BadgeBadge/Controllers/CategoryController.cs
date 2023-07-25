using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Services;
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
		return await _service.Exists(id, name);
	}

	[HttpPost]
	[Route("add")]
	[Authorize]
	public async Task<ApiResponse> AddCategory([FromHeader] string authorization, [FromBody] AddCategoryDto dto)
	{
		try {
			var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
			return await _service.AddCategory(id, dto);
		} catch (ArgumentException) {
			return new UnauthorizedResponse(new BadUserJwtDto());
		}
	}

	[HttpPost]
	[Route("delete")]
	[Authorize]
	public async Task<ApiResponse> DeleteCategory([FromHeader] string authorization, [FromBody] DeleteCategoryDto dto)
	{
		try {
			var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
			return await _service.DeleteCategory(id, dto);
		} catch (ArgumentException) {
			return new UnauthorizedResponse(new BadUserJwtDto());
		}
	}

	[HttpPost]
	[Route("update")]
	[Authorize]
	public async Task<ApiResponse> UpdateCategory([FromHeader] string authorization, [FromBody] UpdateCategoryDto dto)
	{
		try {
			var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
			return await _service.UpdateCategory(id, dto);
		} catch (ArgumentException) {
			return new UnauthorizedResponse(new BadUserJwtDto());
		}
	}

	[HttpPost]
	[Route("merge")]
	public async Task<ApiResponse> MergeCategory([FromHeader] string authorization, [FromBody] MergeCategoryDto dto)
	{
		try {
			var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
			return await _service.MergeCategory(id, dto);
		} catch (ArgumentException) {
			return new UnauthorizedResponse(new BadUserJwtDto());
		}
	}

	[HttpGet]
	[Route("get")]
	public async Task<ApiResponse> GetCategory([FromQuery] int id)
	{
		return await _service.GetCategories(id, false);
	}

	[HttpGet]
	[Route("current")]
	[Authorize]
	public async Task<ApiResponse> GetCurrentCategory([FromHeader] string authorization)
	{
		try {
			var id = TokenUtil.GetUserIdFromJwtBearerToken(authorization);
			return await _service.GetCategories(id, true);
		} catch (ArgumentException) {
			return new UnauthorizedResponse(new BadUserJwtDto());
		}
	}
}