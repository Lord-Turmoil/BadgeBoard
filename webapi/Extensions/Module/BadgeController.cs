using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Extensions.Module
{
	public class BadgeController : Controller
	{
		protected readonly IServiceProvider _provider;

		public BadgeController(IServiceProvider provider)
		{
			_provider = provider;
		}
	}
}
