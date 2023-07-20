namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class CategoryDto
	{
		public string Name { get; set; }
		public CategoryOptionDto Options { get; set; }
	}

	public class CategoryOptionDto
	{
		public bool IsPublic { get; set; }
		public bool AllowAnonymity { get; set; }
		public bool AllowQuestion { get; set; } = true;
		public bool AllowMemory { get; set; } = true;
	}
}
