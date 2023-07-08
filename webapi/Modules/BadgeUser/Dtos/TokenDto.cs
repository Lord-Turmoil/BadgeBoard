using System.Text.Json.Serialization;
using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos
{
	public class TokenDto : ApiRequestDto
	{
		public Guid Id { get; set; }
		public string Password { get; set; }

		public override bool Verify()
		{
			return !(string.IsNullOrEmpty(Id.ToString()) || string.IsNullOrEmpty(Password));
		}
	}

	public class TokenResponseData : ApiResponseData
	{
		public Guid Id { get; set; }
		public string Token { get; set; }

		[JsonIgnore]    // will not present in response
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }

		// Extra information
		[JsonIgnore]
		public bool IsAuthenticated { get; set; }

		[JsonIgnore]
		public int Status { get; set; } = StatusCodes.Status200OK;

		[JsonIgnore]
		public string? Message { get; set; }
	}

	public class TokenFailedDto : OrdinaryDto
	{
		public TokenFailedDto(string? message = "Not authorized") : base(1, message)
		{
		}
	}
}
