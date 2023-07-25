// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeGlobal;

namespace BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge
{
    public class AddBadgeDto : IApiRequestDto
    {
        /// <summary>
        /// User id of sender. 0 if anonymous.
        /// </summary>
        public int SrcId { get; set; }

        /// <summary>
        /// User id of receiver. Must be valid.
        /// </summary>
        public int DstId { get; set; }

        /// <summary>
        /// Badge type, question or memory.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Category id, 0 if is default.
        /// </summary>
        public int Category { get; set; }
        public string? Style { get; set; }

        public bool Verify()
        {
            if (!Models.Badge.Types.IsValid(Type))
            {
                return false;
            }
            if (Style is { Length: > Globals.MaxStyleLength })
            {
                return false;
            }

            return true;
        }

        public IApiRequestDto Format()
        {
            Style = Style?.Trim();
            return this;
        }
    }

    public class AddQuestionBadgeDto : AddBadgeDto
    {
        public string Question { get; set; }
        // Request here won't need Answer, right? :P

        public bool Verify()
        {
            if (!base.Verify())
            {
                return false;
            }

            return Question.Length is > 0 and < Globals.MaxQuestionLength;
        }

        public AddQuestionBadgeDto Format()
        {
            base.Format();
            Question = Question.Trim();
            return this;
        }
    }

    public class AddMemoryBadgeDto : AddBadgeDto
    {
        public string Memory { get; set; }

        public bool Verify()
        {
            if (!base.Verify())
            {
                return false;
            }

            return Memory.Length is > 0 and < Globals.MaxMemoryLength;
        }

        public AddMemoryBadgeDto Format()
        {
            base.Format();
            Memory = Memory.Trim();
            return this;
        }
    }
}
