using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class EditUserService : IEditUserService
	{
		public Task<ApiResponse> EditUserPreference(string jwt, EditUserPreferenceDto dto)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> EditUserInfo(string jwt, EditUserInfoDto dto)
		{
			throw new NotImplementedException();
		}
	}
}
