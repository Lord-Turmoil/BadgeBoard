// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BadgeBoard.Api.Modules.BadgeUser.Models;

public class User
{
    [Key] public int Id { get; set; }

    [ForeignKey(nameof(Id))] public UserAccount Account { get; set; }

    // User self fields
    [Column(TypeName = "varchar(63)")] public string Username { get; set; }

    [Column(TypeName = "varchar(127)")] public string? AvatarUrl { get; set; }

    [Column(TypeName = "varchar(31)")] public string? Title { get; set; }

    public bool IsLocked { get; set; } = false;
    public bool IsAdmin { get; set; } = false;

    // foreign key property
    public int UserPreferenceId { get; set; }

    // reference navigation property
    [ForeignKey(nameof(UserPreferenceId))] public UserPreference Preference { get; set; }

    public int UserInfoId { get; set; }

    [ForeignKey(nameof(UserInfoId))] public UserInfo Info { get; set; }

    // Refresh tokens
    public List<RefreshToken> RefreshTokens { get; set; }

    // Unread record
    public int UnreadRecordId { get; set; }

    [ForeignKey(nameof(UnreadRecordId))] public UnreadRecord Unread { get; set; }


    public static async ValueTask<User> CreateAsync(
        IRepository<User> repo,
        string username,
        UserAccount account,
        UserPreference preference,
        UserInfo info,
        UnreadRecord unread)
    {
        EntityEntry<User> entry = await repo.InsertAsync(new User {
            Username = username,
            Account = account,
            Preference = preference,
            Info = info,
            Unread = unread
        });
        return entry.Entity;
    }


    public static async ValueTask<User> GetAsync(IRepository<User> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException("User");
    }


    public static async ValueTask<User?> FindAsync(IRepository<User> repo, int id)
    {
        return await repo.FindAsync(id);
    }


    /// <summary>
    ///     Get all related entities, since the Arch guys didn't implement this.
    /// </summary>
    /// <param name="unitOfWork">Global unit of work</param>
    /// <param name="user">The user to include related entities</param>
    /// <returns></returns>
    public static async ValueTask<User> IncludeAsync(IUnitOfWork unitOfWork, User user)
    {
        user.Account = await UserAccount.GetAsync(unitOfWork.GetRepository<UserAccount>(), user.Id);
        user.Preference =
            await UserPreference.GetAsync(unitOfWork.GetRepository<UserPreference>(), user.UserPreferenceId);
        user.Info = await UserInfo.GetAsync(unitOfWork.GetRepository<UserInfo>(), user.UserInfoId);
        user.Unread = await UnreadRecord.GetAsync(unitOfWork.GetRepository<UnreadRecord>(), user.UnreadRecordId);
        return user;
    }
}

[Owned]
public class UserPreference
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public bool IsDefaultPublic { get; set; } = false;


    public static async ValueTask<UserPreference> CreateAsync(IRepository<UserPreference> repo)
    {
        EntityEntry<UserPreference> entry = await repo.InsertAsync(new UserPreference());
        return entry.Entity;
    }


    public static async ValueTask<UserPreference> GetAsync(IRepository<UserPreference> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException("Preference");
    }
}

public static class UserSex
{
    public const int Unknown = 0;
    public const int Male = 1;
    public const int Female = 2;


    public static bool IsValid(int sex)
    {
        return sex is >= Unknown and <= Female;
    }
}

[Owned]
public class UserInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "varchar(127)")] public string? Motto { get; set; }

    [Column(TypeName = "varchar(15)")] public string? Birthday { get; set; }

    public int? Sex { get; set; }


    public static async ValueTask<UserInfo> CreateAsync(IRepository<UserInfo> repo)
    {
        EntityEntry<UserInfo> entry = await repo.InsertAsync(new UserInfo());
        return entry.Entity;
    }


    public static async ValueTask<UserInfo> GetAsync(IRepository<UserInfo> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException("UserInfo");
    }
}