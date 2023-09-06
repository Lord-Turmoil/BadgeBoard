// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

public class UpdateBadgeDto : IApiRequestDto
{
    public int Id { get; set; }
    public string? Style { get; set; }
    public bool? IsPublic { get; set; }


    public bool Verify()
    {
        return Style is not { Length: > Globals.MaxStyleLength };
    }


    public IApiRequestDto Format()
    {
        Style = Style?.Trim();
        return this;
    }
}

public class UpdateBadgeSuccessDto : ApiResponseData
{
    public string? Style { get; set; }
    public bool IsPublic { get; set; }
}

public class WrongBadgeTypeDto : OrdinaryDto
{
    public WrongBadgeTypeDto(string? message = "Wrong badge type")
        : base(Errors.WrongBadgeType, message) { }
}

public class NotYourBadgeDto : OrdinaryDto
{
    public NotYourBadgeDto(string? message = "This is not your badge")
        : base(Errors.NotYourBadge, message) { }
}