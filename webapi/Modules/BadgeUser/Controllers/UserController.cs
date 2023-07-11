using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : BaseController<UserController>
	{
		private IUserService _service;

		public UserController(ILogger<UserController> logger, IUserService service) : base(logger)
		{
			_service = service;
		}

		[HttpGet]
		[Route("exists")]
		public async Task<ApiResponse> Exists([FromQuery] string type, [FromQuery] string value)
		{
			return await _service.Exists(type, value);
		}

		[HttpPost]
		[Route("preference")]
		[Authorize]
		public async Task<ApiResponse> UpdatePreference([FromHeader] string authorization,
			[FromBody] UpdateUserPreferenceDto dto)
		{
			var id = TokenUtil.TryGetUserIdFromJwtBearerToken(authorization);
			if (id == null) {
				return new UnauthorizedResponse(new BadUserJwtDto());
			}

			return await _service.UpdatePreference((int)id, dto);
		}


		[HttpPost]
		[Route("info")]
		[Authorize]
		public async Task<ApiResponse> UpdateInfo([FromHeader] string authorization, [FromBody] UpdateUserInfoDto dto)
		{
			var id = TokenUtil.TryGetUserIdFromJwtBearerToken(authorization);
			if (id == null) {
				return new UnauthorizedResponse(new BadUserJwtDto());
			}

			return await _service.UpdateInfo((int)id, dto);
		}


		[HttpGet]
		[Route("user")]
		public async Task<ApiResponse> GetUser([FromQuery] int id)
		{
			if (id < 0) {
				return new BadRequestResponse(new BadRequestDto("Bad ID"));
			}

			return await _service.GetUser(id);
		}
	}
}