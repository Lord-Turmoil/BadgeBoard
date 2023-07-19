namespace BadgeBoard.Api.Extensions.Response
{
	public class ApiRequestDto
	{
		public virtual bool Verify()
		{
			return true;
		}

		public virtual ApiRequestDto Format()
		{
			return this;
		}
	}
}
