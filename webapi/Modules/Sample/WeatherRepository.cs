using BadgeBoard.Api.Extensions.UnitOfWork;
using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.Sample
{
    public class WeatherRepository : Repository<Weather>
    {
        public WeatherRepository(BadgeContext context) : base(context)
        {
        }
    }
}
