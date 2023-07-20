using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class CancelDto : IApiRequestDto
	{
		public List<string> Users { get; set; } = new List<string>();
		public bool Verify()
		{
			return true;
		}

		public IApiRequestDto Format()
		{
			return this;
		}
	}
}
