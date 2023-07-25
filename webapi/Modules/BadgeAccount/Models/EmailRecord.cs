// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
	public static class EmailTypes
	{
		public const int Invalid = 0;
		public const int Register = 1;
		public const int Retrieve = 2;
	}

	public class EmailRecord
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public DateTime Expire { get; set; }

		[Required]
		[Column(TypeName = "varchar(15)")]
		public string Code { get; set; } = string.Empty;

		[Required]
		public int Type { get; set; } = EmailTypes.Invalid;

		[Required]
		public bool IsValid { get; set; } = true;
	}
}
