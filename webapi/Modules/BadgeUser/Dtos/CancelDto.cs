using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class CancelDto : ApiRequestDto
	{
		public List<string> Users { get; set; } = new List<string>();
	}
}
