// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Runtime.Serialization;

namespace BadgeBoard.Api.Modules.BadgeGlobal.Exceptions
{
	public class MissingReferenceException : Exception
	{
		public MissingReferenceException(string message) : base(message) { }
	}
}
