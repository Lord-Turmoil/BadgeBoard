namespace BadgeBoard.Api.Extensions.Response
{
	public interface IApiRequestDto
	{
		public bool Verify();

		public IApiRequestDto Format();
	}
}
