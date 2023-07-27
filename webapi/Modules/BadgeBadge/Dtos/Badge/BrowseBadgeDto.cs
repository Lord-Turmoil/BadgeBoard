using System.Text.Json.Serialization;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

public class BrowseBadgeDto : IApiRequestDto
{
    // target user id
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string Timestamp { get; set; }

    [JsonIgnore] public DateTime BeforeTime { get; set; }
    [JsonIgnore] private bool BadTimestamp { get; set; }


    public bool Verify()
    {
        return !BadTimestamp;
    }


    public IApiRequestDto Format()
    {
        try
        {
            BeforeTime = BadgeDtoUtil.ParseTimestamp(Timestamp);
            BadTimestamp = false;
        }
        catch (FormatException ex)
        {
            BadTimestamp = true;
        }

        return this;
    }
}



public class BrowseBadgeSuccessDto : ApiResponseData
{
    public int Count { get; set; }
    public IList<BadgeDto> Badges { get; set; }
}