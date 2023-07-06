﻿using BadgeBoard.Api.Extensions.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models
{
	public class UserAccount
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Column(TypeName = "varchar(63)")]
		[EmailAddress]
		[Required]
		public string Email { get; set; } = string.Empty;

		[Column(TypeName = "varbinary(256)")]
		[Required]
		public byte[] Password { get; set; } = Array.Empty<byte>();

		[Column(TypeName = "varbinary(256)")]
		[Required]
		public byte[] Salt { get; set; } = Array.Empty<byte>();

		public static UserAccount Create(IRepository<UserAccount> repo, byte[] salt, byte[] password, string email = "")
		{
			return repo.Insert(new UserAccount { Salt = salt, Password = password, Email = email });
		}
	}
}
