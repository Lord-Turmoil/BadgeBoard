﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;

public class UpdateCategoryDto : BaseCategoryDto, IApiRequestDto
{
    public int Id { get; set; }


    public bool Verify()
    {
        if (Name != null)
        {
            return Name.Length is > 0 and < Globals.MaxCategoryNameLength;
        }

        return true;
    }


    public IApiRequestDto Format()
    {
        Name = Name?.Trim();
        return this;
    }
}