using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category
{
    public class MergeCategoryDto : IApiRequestDto
    {
		// 0 means default category
		public int SrcId { get; set; }
		public int DstId { get; set; }
		public bool Delete { get; set; } // whether delete source

		public bool Verify()
		{
			return true;
		}

	    public IApiRequestDto Format()
	    {
		    return this;
	    }
    }
}
