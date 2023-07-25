// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Modules.BadgeGlobal.Utils;

public static class KeyGenerator
{
    private const int MinUserId = 1000000000;
    private const int MaxUserId = 2000000000;


    public static int GenerateKey()
    {
        return (int)Random.Shared.NextInt64(MinUserId, MaxUserId + 1);
    }
}