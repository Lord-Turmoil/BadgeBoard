namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class DeleteBadgeDto
	{
		public List<int> Badges { get; set; } = new List<int>();
		
		// Admin can force delete a badge. :)
		public bool Force { get; set; }
	}
}
