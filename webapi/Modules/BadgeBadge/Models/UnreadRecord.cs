// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BadgeBoard.Api.Modules.BadgeBadge.Models;

public class UnreadRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int QuestionCount { get; set; }
    public int MemoryCount { get; set; }


    public static async Task<UnreadRecord> CreateAsync(
        IRepository<UnreadRecord> repo,
        int questionCount = 0,
        int memoryCount = 0)
    {
        EntityEntry<UnreadRecord> entry = await repo.InsertAsync(new UnreadRecord {
            QuestionCount = questionCount,
            MemoryCount = memoryCount
        });
        return entry.Entity;
    }


    public static async Task<UnreadRecord?> FindAsync(IRepository<UnreadRecord> repo, int id)
    {
        return await repo.FindAsync(id);
    }


    public static async Task<UnreadRecord> GetAsync(IRepository<UnreadRecord> repo, int id)
    {
        return await repo.FindAsync(id) ?? throw new MissingReferenceException("UnreadRecord");
    }
}