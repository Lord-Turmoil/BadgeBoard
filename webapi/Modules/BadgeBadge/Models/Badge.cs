// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeGlobal.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BadgeBoard.Api.Modules.BadgeBadge.Models;

public class Badge : TimeRecordModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Identify different payloads
    public int Type { get; set; }
    public int PayloadId { get; set; }

    // Users
    public int Sender { get; set; }
    public int Receiver { get; set; }

    // For CSS style class
    [Column(TypeName = "varchar(31)")] public string? Style { get; set; }

    // Restrictions
    public bool IsLocked { get; set; }
    public bool IsPublic { get; set; } = true;

    // check mark
    public bool IsChecked { get; set; } = false;

    // Foreign keys
    public int? CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))] public Category? Category { get; set; }

    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))] public User User { get; set; }


    public static async Task<Badge> CreateAsync(
        IRepository<Badge> repo,
        int type,
        int payload,
        User? sender,
        User receiver,
        Category? category = null,
        string? style = null,
        bool isLocked = false,
        bool isPublic = true)
    {
        var timestamp = DateTime.Now;
        EntityEntry<Badge> entry = await repo.InsertAsync(new Badge {
            Type = type,
            PayloadId = payload,
            Sender = sender?.Id ?? 0,
            Receiver = receiver.Id,
            Style = style,
            IsLocked = isLocked,
            IsPublic = isPublic,
            Category = category,
            User = receiver,
            CreatedTime = timestamp,
            UpdatedTime = timestamp
        });
        return entry.Entity;
    }


    public static async Task<Badge?> FindAsync(IRepository<Badge> repo, int id)
    {
        return await repo.FindAsync(id);
    }


    public static class Types
    {
        public const int Question = 1;
        public const int Memory = 2;


        public static bool IsValid(int type)
        {
            return type is Question or Memory;
        }
    }
}

public class QuestionPayload
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "text")] public string Question { get; set; } = "";

    [Column(TypeName = "text")] public string? Answer { get; set; }


    public static async Task<QuestionPayload> CreateAsync(
        IRepository<QuestionPayload> repo,
        string question = "",
        string? answer = null)
    {
        EntityEntry<QuestionPayload> entry = await repo.InsertAsync(new QuestionPayload {
            Question = question,
            Answer = answer
        });
        return entry.Entity;
    }


    public static async Task<QuestionPayload> GetAsync(IRepository<QuestionPayload> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException($"QuestionPayload {id}");
    }


    public static async Task<QuestionPayload?> FindAsync(IRepository<QuestionPayload> repo, int id)
    {
        return await repo.FindAsync(id);
    }
}

public class MemoryPayload
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "text")] public string Memory { get; set; } = "";


    public static async Task<MemoryPayload> CreateAsync(IRepository<MemoryPayload> repo, string memory = "")
    {
        EntityEntry<MemoryPayload> entry = await repo.InsertAsync(new MemoryPayload { Memory = memory });
        return entry.Entity;
    }


    public static async Task<MemoryPayload> GetAsync(IRepository<MemoryPayload> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException($"MemoryPayload {id}");
    }


    public static async Task<MemoryPayload?> FindAsync(IRepository<MemoryPayload> repo, int id)
    {
        return await repo.FindAsync(id);
    }
}