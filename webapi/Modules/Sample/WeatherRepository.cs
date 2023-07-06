using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.Sample.Models;

namespace BadgeBoard.Api.Modules.Sample
{
    public class WeatherRepository : Repository<Weather>
    {
        public WeatherRepository(BadgeContext context) : base(context)
        {
        }
    }
}
