// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeGlobal.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BadgeBoard.Api.Modules.BadgeBadge.Models;

public class Category : TimeRecordModel
{
    [Key] public int Id { get; init; }

    [Column(TypeName = "varchar(127)")]
    [Required]
    public string Name { get; set; }

    public int Size { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }

    public int CategoryOptionId { get; set; }

    [ForeignKey("CategoryOptionId")] public CategoryOption Option { get; set; }


    public static async Task<Category> CreateAsync(IRepository<Category> repo, string Name, User user,
        CategoryOption option)
    {
        DateTime now = DateTime.Now;
        EntityEntry<Category> entry = await repo.InsertAsync(new Category {
            Id = KeyGenerator.GenerateKey(),
            Name = Name,
            Size = 0,
            User = user,
            Option = option,
            CreatedTime = now,
            UpdatedTime = now
        });
        return entry.Entity;
    }


    public static async Task<Category> GetAsync(IRepository<Category> repo, int id, bool include = false)
    {
        if (include)
            return await repo.GetFirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: source => source.Include(x => x.Option),
                disableTracking: false
            ) ?? throw new MissingReferenceException("Category");

        return await repo.FindAsync(id) ?? throw new MissingReferenceException("Category");
    }


    public static async Task<Category?> FindAsync(IRepository<Category> repo, int id, bool include = false)
    {
        if (include)
            return await repo.GetFirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: source => source.Include(x => x.Option),
                disableTracking: false
            );

        return await repo.FindAsync(id);
    }
}

[Owned]
public class CategoryOption
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Whether visible to everyone
    public bool IsPublic { get; set; } = true;

    // Whether allow anonymous post
    public bool AllowAnonymity { get; set; } = true;

    // What type of badge that is allowed for visitor
    // For the owner him/her self, any type is allowed
    public bool AllowQuestion { get; set; } = true;
    public bool AllowMemory { get; set; } = true;


    public static async Task<CategoryOption> CreateAsync(IRepository<CategoryOption> repo)
    {
        EntityEntry<CategoryOption> entry = await repo.InsertAsync(new CategoryOption());
        return entry.Entity;
    }
}