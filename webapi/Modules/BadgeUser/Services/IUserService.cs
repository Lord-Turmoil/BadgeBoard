using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IUserService
	{
		// Get user status
		public Task<ApiResponse> Exists(string type, string value);

		// set user properties
		public Task<ApiResponse> UpdatePreference(int id, UpdateUserPreferenceDto dto);

		public Task<ApiResponse> UpdateInfo(int id, UpdateUserInfoDto dto);

		public Task<ApiResponse> UpdateUsername(int id, UpdateUsernameDto dto);

		// get user properties
		public Task<ApiResponse> GetUser(int id);

		// get current user
		public Task<ApiResponse> GetCurrentUser(int id);
	}
}
