namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos
{
	public class CategoryDto
	{
		public string Name { get; set; }
		public CategoryOptionDto Option { get; set; }
	}

	public class CategoryOptionDto
	{
		public bool IsPublic { get; set; }
		public bool AllowAnonymity { get; set; }
		public bool AllowQuestion { get; set; }
		public bool AllowMemory { get; set; }
	}
}
