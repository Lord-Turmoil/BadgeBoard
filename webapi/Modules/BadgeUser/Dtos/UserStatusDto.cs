using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class UserAlreadyExistsDto : OrdinaryDto
	{
		public UserAlreadyExistsDto() : base(1001, "User already exists")
		{
		}
	}

	public class UserNotExistsDto : OrdinaryDto
	{
		public UserNotExistsDto() : base(1002, "User does not exists")
		{
		}
	}
}
