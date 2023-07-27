// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;

public class ResponseException : Exception
{
    public ApiResponse Response { get; private set; }


    public ResponseException(ApiResponse response)
    {
        Response = response;
    }
}