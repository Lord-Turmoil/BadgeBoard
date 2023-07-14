using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using Google.Protobuf.Reflection;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class UserAlreadyExistsDto : OrdinaryDto
	{
		public UserAlreadyExistsDto(string? message = "User already exists") : base(Errors.UserAlreadyExists, message)
		{
		}
	}

	public class UserNotExistsDto : OrdinaryDto
	{
		public UserNotExistsDto() : base(Errors.UserNotExists, "User does not exists")
		{
		}
	}

	public class BadUserJwtDto : OrdinaryDto
	{
		public BadUserJwtDto() : base(Errors.BadUserJwt, "Bad JWT")
		{
		}
	}
}
