﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;

public class CategoryAlreadyExistsDto : OrdinaryDto
{
    public CategoryAlreadyExistsDto(string? message = "Category name already exists") : base(
        Errors.CategoryAlreadyExists, message) { }
}

public class CategoryNotExistsDto : OrdinaryDto
{
    public CategoryNotExistsDto(string? message = "Category does not exit") : base(Errors.CategoryNotExits, message) { }
}

public class CategoryMergeSelfErrorDto : OrdinaryDto
{
    public CategoryMergeSelfErrorDto(string? message = "Cannot merge category to itself") : base(
        Errors.CategoryMergeSelf, message) { }
}