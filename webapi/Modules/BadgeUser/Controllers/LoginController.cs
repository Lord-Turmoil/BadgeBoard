using BadgeBoard.Api.Extensions.Module;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeUser.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : BaseController<LoginController>
	{
		public LoginController(ILogger<LoginController> logger) : base(logger)
		{
		}
	}
}
