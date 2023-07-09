using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
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


		[HttpPost]
		[Route("preference")]
		[Authorize]
		public async Task<ApiResponse> UpdatePreference([FromHeader] string authorization, [FromBody] UpdateUserPreferenceDto dto)
		{
			var id = JwtUtil.GetValueFromBearerToken(authorization);
			if (id == null) {
				return new UnauthorizedResponse(new BadUserJwtDto());
			}

			return await _service.UpdatePreference(new Guid(id), dto);
		}

		
		[HttpPost]
		[Route("info")]
		[Authorize]
		public async Task<ApiResponse> UpdateInfo([FromHeader] string authorization, [FromBody] UpdateUserInfoDto dto)
		{
			var id = JwtUtil.GetValueFromBearerToken(authorization);
			if (id == null) {
				return new UnauthorizedResponse(new BadUserJwtDto());
			}

			return await _service.UpdateInfo(new Guid(id), dto);
		}

	}
}
