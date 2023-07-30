// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;

public static class CategoryUtil
{
    public static async Task<Category> CreateCategoryAsync(IUnitOfWork unitOfWork, string name, User user)
    {
        var option = await CategoryOption.CreateAsync(unitOfWork.GetRepository<CategoryOption>());
        var category = await Category.CreateAsync(unitOfWork.GetRepository<Category>(), name, user, option);
        return category;
    }


    public static async Task<bool> HasCategoryOfUserAsync(IRepository<Category> repo, int id, string name)
    {
        Category? category = await repo.GetFirstOrDefaultAsync(predicate: x => x.UserId == id && x.Name == name);
        return category != null;
    }


    public static async Task<IList<Category>> GetCategoryOfUserAsync(IRepository<Category> repo, int id)
    {
        return await repo.GetAllAsync(predicate: x => x.UserId == id);
    }


    public static Category UpdateCategory(Category category, BaseCategoryDto dto)
    {
        var updated = false;

        if (dto.Name != null)
        {
            if (!dto.Name.Equals(category.Name))
            {
                category.Name = dto.Name;
                updated = true;
            }
        }

        if (dto.Option != null)
        {
            if (dto.Option.IsPublic != category.Option.IsPublic)
            {
                category.Option.IsPublic = dto.Option.IsPublic;
                updated = true;
            }

            if (dto.Option.AllowAnonymity != category.Option.AllowAnonymity)
            {
                category.Option.AllowAnonymity = dto.Option.AllowAnonymity;
                updated = true;
            }

            if (dto.Option.AllowQuestion != category.Option.AllowQuestion)
            {
                category.Option.AllowQuestion = dto.Option.AllowQuestion;
                updated = true;
            }

            if (dto.Option.AllowMemory != category.Option.AllowMemory)
            {
                category.Option.AllowMemory = dto.Option.AllowMemory;
                updated = true;
            }
        }

        if (updated)
        {
            category.UpdatedTime = DateTime.Now;
        }

        return category;
    }


    public static async Task<List<DeleteBadgeErrorData>> EraseCategoryAsync(
        IUnitOfWork unitOfWork, Category category, User user)
    {
        if (category.UserId != user.Id)
        {
            return new List<DeleteBadgeErrorData> {
                new() {
                    Id = 0,
                    Message = $"Category {category.Id} does not belong to user {user.Id}"
                }
            };
        }

        IList<Badge> badges = await unitOfWork.GetRepository<Badge>()
            .GetAllAsync(predicate: x => x.CategoryId == category.Id);
        List<DeleteBadgeErrorData> errors = await BadgeUtil.EraseBadges(unitOfWork, badges, user, true);

        unitOfWork.GetRepository<Category>().Delete(category);
        unitOfWork.GetRepository<CategoryOption>().Delete(category.CategoryOptionId);

        return errors;
    }


    public static async Task<List<DeleteBadgeErrorData>> EraseCategoriesAsync(
        IUnitOfWork unitOfWork, IEnumerable<Category> categories, User user)
    {
        IRepository<Badge> badgeRepo = unitOfWork.GetRepository<Badge>();
        IRepository<Category> categoryRepo = unitOfWork.GetRepository<Category>();
        IRepository<CategoryOption> optionRepo = unitOfWork.GetRepository<CategoryOption>();
        var errors = new List<DeleteBadgeErrorData>();

        foreach (Category category in categories)
        {
            if (category.UserId != user.Id)
            {
                errors.Add(new DeleteBadgeErrorData {
                    Id = 0,
                    Message = $"Category {category.Id} does not belong to user {user.Id}"
                });
                continue;
            }

            IList<Badge> badges = await badgeRepo.GetAllAsync(predicate: x => x.CategoryId == category.Id);
            errors.AddRange(await BadgeUtil.EraseBadges(unitOfWork, badges, user, true));
            categoryRepo.Delete(category);
            optionRepo.Delete(category.CategoryOptionId);
        }

        return errors;
    }


    // src and dst should not be both null (although cause redundant work only)
    public static async Task MergeCategoriesAsync(IUnitOfWork unitOfWork, Category? src, Category? dst)
    {
        IRepository<Badge> repo = unitOfWork.GetRepository<Badge>();

        IList<Badge> badges;
        if (src == null)
        {
            badges = await repo.GetAllAsync(predicate: x => x.CategoryId == null);
        }
        else
        {
            badges = await repo.GetAllAsync(predicate: x => x.CategoryId == src.Id);
        }

        foreach (Badge badge in badges)
        {
            badge.CategoryId = dst?.Id;
        }

        if (dst != null)
        {
            dst.UpdatedTime = DateTime.Now;
        }
    }
}