// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Extensions.Response;

public interface IApiRequestDto
{
    public bool Verify();

    public IApiRequestDto Format();
}