using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.Sample.Repository
{
	public class WeatherRepository : Repository<Weather>
	{
		public WeatherRepository(DbContext context) : base(context)
		{
		}
	}
}
