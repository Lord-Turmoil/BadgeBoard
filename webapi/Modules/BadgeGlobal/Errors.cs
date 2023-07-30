// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Modules.BadgeGlobal;

public static class Errors
{
    // Internal server errors
    public const int FailedToSaveChanges = 6601;
    public const int MissingReference = 6602;
    public const int UnexpectedError = 6699;

    // User status
    public const int UserAlreadyExists = 1001;
    public const int UserNotExists = 1002;
    public const int BadUserJwt = 1003;

    // Login
    public const int WrongPassword = 2001;
    public const int GetTokenRejected = 2002;
    public const int RefreshTokenRejected = 2003;
    public const int RevokeTokenRejected = 2004;

    // User Info
    public const int DeleteAvatarError = 3001;
    public const int SaveAvatarError = 3002;

    // Category
    public const int CategoryAlreadyExists = 4001;
    public const int CategoryNotExits = 4002;
    public const int CategoryMergeSelf = 4003;
    public const int CategoryNotMatchUser = 4004;
    public const int CategoryIsPrivate = 4005;
    public const int BadgeTypeNotAllowed = 4006;
    public const int AnonymousBadgeNotAllowed = 4007;

    // Badge
    public const int FailedToGetBadge = 5001;
    public const int BadgeNotExists = 5002;
    public const int PayloadNotExists = 5003;
    public const int BadgeIsPrivate = 5004;
    public const int BadgeCorrupted = 5005;
    public const int WrongBadgeType = 5006;
}