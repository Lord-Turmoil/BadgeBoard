// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;

public class BadJwtTokenException : ArgumentException
{
    public BadJwtTokenException(string? message) : base(message) { }
}