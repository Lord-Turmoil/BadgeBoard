// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;

public class BadgeNotExistsDto : OrdinaryDto
{
    public BadgeNotExistsDto(string? message = "Badge does no exists") : base(Errors.BadgeNotExists, message) { }
}

public class PayloadNotExistsDto : OrdinaryDto
{
    public PayloadNotExistsDto(string? message = "Payload does not exists") : base(Errors.PayloadNotExists, message) { }
}

public class BadgeIsPrivateDto : OrdinaryDto
{
    public BadgeIsPrivateDto(string? message = "Required badge is private") : base(Errors.BadgeIsPrivate, message) { }
}

public class BadgeCorruptedDto : BadDto
{
    public BadgeCorruptedDto(string? message = "Badge corrupted", object? data = null)
        : base(Errors.BadgeCorrupted, message, data) { }
}