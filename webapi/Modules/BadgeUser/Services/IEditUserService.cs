using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IEditUserService
	{
		public Task<ApiResponse> EditUserPreference(string jwt, EditUserPreferenceDto dto);
		public Task<ApiResponse> EditUserInfo(string jwt, EditUserInfoDto dto);
	}
}
