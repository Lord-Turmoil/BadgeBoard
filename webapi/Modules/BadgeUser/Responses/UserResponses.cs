// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeUser.Responses;

public class CodeSentResponse : GoodResponse
{
    public CodeSentResponse(object? value) : base(value) { }
}