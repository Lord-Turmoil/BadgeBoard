// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;

public class GetCategoryDto : ApiResponseData
{
	public List<CategoryDto> Categories { get; set; } = new();
}