// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
	/// <summary>
	/// Only update answer.
	/// </summary>
	public class UpdateQuestionBadgeDto : IApiRequestDto
	{
		public int Id { get; set; }
		public string? Answer { get; set; }

		public bool Verify()
		{
			if (Answer != null) {
				return Answer.Length is > 0 and < Globals.MaxAnswerLength;
			}

			return true;
		}

		public IApiRequestDto Format()
		{
			Answer = Answer?.Trim();
			return this;
		}
	}

	public class UpdateQuestionBadgeSuccessDto : ApiResponseData
	{
		public string? Answer { get; set; }
	}
}
