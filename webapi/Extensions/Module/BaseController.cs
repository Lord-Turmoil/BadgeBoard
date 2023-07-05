using BadgeBoard.Api.Modules.Sample.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BaseController<TController> : Controller where TController : Controller
	{
		protected readonly ILogger<TController> _logger;

		public BaseController(ILogger<TController> logger)
		{
			_logger = logger;
		}
	}
}
