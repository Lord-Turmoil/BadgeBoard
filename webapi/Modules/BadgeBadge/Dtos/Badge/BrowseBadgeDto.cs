// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Text.Json.Serialization;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

public class BrowseBadgeDto : IApiRequestDto
{
    public string Timestamp { get; set; }

    [JsonIgnore] public DateTime BeforeTime { get; set; }
    [JsonIgnore] private bool BadTimestamp { get; set; }


    public virtual bool Verify()
    {
        return !BadTimestamp;
    }


    public virtual IApiRequestDto Format()
    {
        try
        {
            BeforeTime = BadgeDtoUtil.ParseTimestamp(Timestamp);
            BadTimestamp = false;
        }
        catch (FormatException)
        {
            BadTimestamp = true;
        }

        return this;
    }
}

public class BrowseAllBadgeDto : BrowseBadgeDto
{
    // target user id
    public int UserId { get; set; }
}

public class BrowseCategoryBadgeDto : BrowseBadgeDto
{
    public int CategoryId { get; set; }
}

public class BrowseBadgeSuccessDto : ApiResponseData
{
    public int Count { get; set; }
    public IList<BadgeDto> Badges { get; set; }
}