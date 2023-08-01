// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;

public class DeleteCategoryDto : IApiRequestDto
{
    public List<int> Categories { get; set; }


    public bool Verify()
    {
        return true;
    }


    public IApiRequestDto Format()
    {
        return this;
    }
}

public class DeleteCategorySuccessDto : ApiResponseData
{
    public List<DeleteErrorData> Errors { get; set; } = new();
}