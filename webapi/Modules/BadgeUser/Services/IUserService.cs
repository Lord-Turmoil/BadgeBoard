using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IUserService
	{
		// Get user status
		public Task<ApiResponse> Exists(string type, string value);

		// set user properties
		public Task<ApiResponse> UpdatePreference(Guid id, UpdateUserPreferenceDto dto);

		public Task<ApiResponse> UpdateInfo(Guid id, UpdateUserInfoDto dto);
	}
}
