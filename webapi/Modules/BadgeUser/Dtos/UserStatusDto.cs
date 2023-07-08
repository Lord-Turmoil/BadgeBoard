using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class UserAlreadyExistsDto : OrdinaryDto
	{
		public UserAlreadyExistsDto() : base(Errors.UserAlreadyExists, "User already exists")
		{
		}
	}

	public class UserNotExistsDto : OrdinaryDto
	{
		public UserNotExistsDto() : base(Errors.UserNotExists, "User does not exists")
		{
		}
	}
}
