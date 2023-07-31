// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services;

public class CategoryService : BaseService, ICategoryService
{
    public CategoryService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
        : base(provider, unitOfWork, mapper) { }


    public async Task<ApiResponse> Exists(int id, string name)
    {
        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        var exists = await CategoryUtil.HasCategoryOfUserAsync(_unitOfWork.GetRepository<Category>(), user.Id, name);
        return new GoodResponse(new GoodWithDataDto(exists));
    }


    public async Task<ApiResponse> AddCategory(int id, AddCategoryDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        // dto.Name is checked in dto.Verify(), it is not null here.
        if (await CategoryUtil.HasCategoryOfUserAsync(repo, id, dto.Name!))
        {
            return new GoodResponse(new CategoryAlreadyExistsDto());
        }

        Category category = await CategoryUtil.CreateCategoryAsync(_unitOfWork, dto.Name!, user);
        CategoryUtil.UpdateCategory(category, dto);
        await _unitOfWork.SaveChangesAsync();

        CategoryDto? data = _mapper.Map<Category, CategoryDto>(category);
        return new GoodResponse(new GoodDto("New category options", data));
    }


    public async Task<ApiResponse> DeleteCategory(int id, DeleteCategoryDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        var data = new DeleteCategorySuccessDto();
        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        foreach (var categoryId in dto.Categories)
        {
            // check existence
            Category? category = await Category.FindAsync(repo, categoryId);
            if (category == null)
            {
                data.Errors.Add(new DeleteCategoryErrorData {
                    Id = categoryId,
                    Message = "Not exists"
                });
                continue;
            }

            if (category.IsDefault)
            {
                data.Errors.Add(new DeleteCategoryErrorData {
                    Id = categoryId,
                    Message = "Cannot delete default category"
                });
            }

            await CategoryUtil.EraseCategoryAsync(_unitOfWork, category, user);
        }

        await _unitOfWork.SaveChangesAsync();


        var message = data.Errors.Count > 0 ? "Deletion partial succeeded" : "Deletion complete";

        return new GoodResponse(new GoodDto(message, data));
    }


    public async Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        Category? category = await Category.FindAsync(repo, dto.Id, true);
        if (category == null)
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        // Check name duplication
        if (dto.Name != null && category.Name != dto.Name)
        {
            if (await CategoryUtil.HasCategoryOfUserAsync(repo, id, dto.Name))
            {
                return new GoodResponse(new CategoryAlreadyExistsDto());
            }
        }

        // Update whole category.
        CategoryUtil.UpdateCategory(category, dto);
        await _unitOfWork.SaveChangesAsync();

        CategoryDto? data = _mapper.Map<Category, CategoryDto>(category);

        return new GoodResponse(new GoodDto("Category updated", data));
    }


    public async Task<ApiResponse> MergeCategory(int id, MergeCategoryDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        if (dto.SrcId == dto.DstId)
        {
            return new BadRequestResponse(new CategoryMergeSelfErrorDto());
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        Category? src = await Category.FindAsync(repo, dto.SrcId);
        if (src == null)
        {
            return new GoodResponse(new CategoryNotExistsDto("Source category not exists"));
        }
        Category? dst = await Category.FindAsync(repo, dto.DstId);
        if (dst == null)
        {
            return new GoodResponse(new CategoryNotExistsDto("Destination category not exists"));
        }

        await CategoryUtil.MergeCategoriesAsync(_unitOfWork, src, dst);
        if (dto.Delete && !src.IsDefault)
        {
            await CategoryUtil.EraseCategoryAsync(_unitOfWork, src, user);
        }

        await _unitOfWork.SaveChangesAsync();

        return new GoodResponse(new GoodDto("Categories Merged"));
    }


    public async Task<ApiResponse> GetCategories(int userId)
    {
        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), userId);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        IList<Category> categoryList = await repo.GetAllAsync(
            predicate: x => x.UserId == userId,
            include: source => source.Include(x => x.Option),
            orderBy: source => source.OrderBy(x => x.Name));
        var data = new GetCategorySuccessDto();
        foreach (Category category in categoryList)
        {
            if (category.Option.IsPublic)
            {
                data.Categories.Add(_mapper.Map<Category, CategoryDto>(category));
            }
        }

        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetCategories(int id, int userId)
    {
        IRepository<User> userRepo = _unitOfWork.GetRepository<User>();
        User? initiator = await User.FindAsync(userRepo, id);
        if (initiator == null)
        {
            return new GoodResponse(new UserNotExistsDto("Initiator is missing"));
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        IList<Category> categoryList = await repo.GetAllAsync(
            predicate: x => x.UserId == userId,
            include: source => source.Include(x => x.Option),
            orderBy: source => source.OrderBy(x => x.Name));

        var data = new GetCategorySuccessDto();
        foreach (Category category in categoryList)
        {
            if (category.Option.IsPublic || category.UserId == id || user.IsAdmin)
            {
                data.Categories.Add(_mapper.Map<Category, CategoryDto>(category));
            }
        }

        return new GoodResponse(new GoodWithDataDto(data));
    }
}