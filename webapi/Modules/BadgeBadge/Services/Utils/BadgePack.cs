// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Modules.BadgeBadge.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;

public abstract class BadgePack
{
    public Badge Badge { get; set; }
}

public class QuestionBadgePack : BadgePack
{
    public QuestionPayload Payload { get; set; }
}

public class MemoryBadgePack : BadgePack
{
    public MemoryPayload Payload { get; set; }
}