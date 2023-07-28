// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeGlobal.Dtos;

public class UnexpectedErrorDto : ApiResponseDto
{
    public UnexpectedErrorDto(Exception ex) : base(Errors.UnexpectedError, ex.Message, ex) { }
}