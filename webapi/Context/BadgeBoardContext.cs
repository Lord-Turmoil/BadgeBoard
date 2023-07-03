using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Context
{
	public class BadgeBoardContext : DbContext
	{
		public BadgeBoardContext(DbContextOptions<BadgeBoardContext> options) : base(options)
		{
		}
	}
}
