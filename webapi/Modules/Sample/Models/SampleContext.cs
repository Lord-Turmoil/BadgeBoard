using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.Sample.Models
{
	public class SampleContext : DbContext
	{
		public SampleContext(DbContextOptions<SampleContext> options) : base(options)
		{
		}
	}
}
