using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category
{
    public class DeleteCategoryDto : IApiRequestDto
    {
        public List<int> categories;

        public bool Verify()
        {
            return true;
        }

        public IApiRequestDto Format()
        {
            return this;
        }
    }

    public class DeleteCategoryErrorData
    {
        public int Id { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteCategorySuccessDto : ApiResponseData
    {
        public List<DeleteCategoryErrorData> Errors { get; set; } = new();
    }
}
