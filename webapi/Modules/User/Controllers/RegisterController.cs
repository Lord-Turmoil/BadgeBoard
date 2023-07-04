using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.User.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        [HttpPost]
        [Route("code")]
        public async Task<BadgeResponse> SendVerificationCode([FromQuery] string? type)
        {
			int emailType = EmailTypes.Invalid;
            if ("register".Equals(type)) {
				emailType = EmailTypes.Register;
            } else if ("retrieve".Equals(type)) {
				emailType = EmailTypes.Retrieve;
            }

            if (emailType == EmailTypes.Invalid) {
                return new BadRequestResponse(new BadRequestDto($"Invalid type: {type}"));
            }

            return new GoodResponse(new GoodDto());
		}
    }
}
