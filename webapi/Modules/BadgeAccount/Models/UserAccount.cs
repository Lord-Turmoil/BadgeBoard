// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BadgeBoard.Api.Modules.BadgeAccount.Models;

[Owned]
public class UserAccount
{
    [Key] public int Id { get; set; }

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


    public static async Task<UserAccount> CreateAsync(IRepository<UserAccount> repo, byte[] salt, byte[] password,
        string email = "")
    {
        EntityEntry<UserAccount> entry = await repo.InsertAsync(new UserAccount {
            Id = AccountUtil.GenerateAccountId(),
            Salt = salt,
            Password = password,
            Email = email
        });
        return entry.Entity;
    }


    public static async Task<UserAccount> GetAsync(IRepository<UserAccount> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException("Account");
    }
}