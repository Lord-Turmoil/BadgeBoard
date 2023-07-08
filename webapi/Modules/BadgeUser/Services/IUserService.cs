using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public interface IUserService
	{
		// Get user status
		public Task<ApiResponse> Exists(string type, string value);

		// set user properties

	}
}
