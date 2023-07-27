// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeUser.Dtos;

public class UserAlreadyExistsDto : OrdinaryDto
{
    public UserAlreadyExistsDto(string? message = "User already exists") : base(Errors.UserAlreadyExists, message) { }
}

public class UserNotExistsDto : OrdinaryDto
{
    public UserNotExistsDto(string? message = "User does not exists") : base(Errors.UserNotExists, message) { }
}

public class BadUserJwtDto : OrdinaryDto
{
    public BadUserJwtDto() : base(Errors.BadUserJwt, "Bad JWT") { }
}