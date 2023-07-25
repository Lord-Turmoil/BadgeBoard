// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Text.Json.Serialization;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

public class BadgeDto
{
    public int Id { get; set; }
    public int Type { get; set; }

    public bool IsChecked { get; set; }

    [JsonIgnore] public int Sender { get; set; }

    public UserBriefDto? SrcUser { get; set; }

    [JsonIgnore] public int Receiver { get; set; }

    public UserBriefDto DstUser { get; set; }

    public string? Style { get; set; }

    public CategoryBriefDto? Category { get; set; }
}

public class QuestionBadgeDto : BadgeDto
{
    public QuestionPayloadDto Payload { get; set; }
}

public class MemoryBadgeDto : BadgeDto
{
    public MemoryPayloadDto Payload { get; set; }
}

public class QuestionPayloadDto : BadgeDto
{
    public string Question { get; set; }
    public string Answer { get; set; }
}

public class MemoryPayloadDto
{
    public string Memory { get; set; }
}