// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeGlobal.Dtos
{
	public class FailedToSaveChangesDto : ApiResponseDto
	{
		public FailedToSaveChangesDto(string? message = "Failed to save changes", object? data = null) : base(Errors.FailedToSaveChanges, message, data)
		{
		}
	}

	public class MissingReferenceDto: ApiResponseDto
	{
		public MissingReferenceDto(string? message = "Missing reference", object? data = null) : base(Errors.MissingReference, message, data)
		{
		}
	}
}
