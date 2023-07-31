// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;

public class CategoryDto
{
    public int Id { get; set; }
    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public CategoryOptionDto Option { get; set; }
}

public class CategoryOptionDto
{
    public bool IsPublic { get; set; }
    public bool AllowAnonymity { get; set; }
    public bool AllowQuestion { get; set; }
    public bool AllowMemory { get; set; }
}

public class CategoryBriefDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class BaseCategoryDto
{
    public string? Name { get; set; }
    public CategoryOptionDto? Option { get; set; }
}