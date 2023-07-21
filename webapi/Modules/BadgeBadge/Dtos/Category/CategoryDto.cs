namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
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

    public class CategoryBriefDto
    {
        public int Id { get; set; }
	    public string Name { get; set; }
    }
}
