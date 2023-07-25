// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
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
        if (user == null) return new GoodResponse(new UserNotExistsDto());

        var exists = await CategoryUtil.HasCategoryOfUserAsync(_unitOfWork.GetRepository<Category>(), user.Id, name);
        return new GoodResponse(new GoodWithDataDto(exists));
    }


    public async Task<ApiResponse> AddCategory(int id, AddCategoryDto dto)
    {
        if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null) return new GoodResponse(new UserNotExistsDto());

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        // dto.Name is checked in dto.Verify(), it is not null here.
        if (await CategoryUtil.HasCategoryOfUserAsync(repo, id, dto.Name!))
            return new GoodResponse(new CategoryAlreadyExistsDto());

        Category category = await CategoryUtil.CreateCategoryAsync(_unitOfWork, dto.Name!, user);
        CategoryUtil.UpdateCategory(category, dto);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
        }

        CategoryDto? data = _mapper.Map<Category, CategoryDto>(category);
        return new GoodResponse(new GoodDto("New category options", data));
    }


    public async Task<ApiResponse> DeleteCategory(int id, DeleteCategoryDto dto)
    {
        if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null) return new GoodResponse(new UserNotExistsDto());

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

            // do merge to default
            if (dto.Merge) await CategoryUtil.MergeCategoriesAsync(_unitOfWork, category, null);
            await CategoryUtil.EraseCategoryAsync(_unitOfWork, category, user);
        }

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
        }

        var message = data.Errors.Count > 0 ? "Deletion partial succeeded" : "Deletion complete";

        return new GoodResponse(new GoodDto(message, data));
    }


    public async Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDto dto)
    {
        if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null) return new GoodResponse(new UserNotExistsDto());

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        Category? category = await Category.FindAsync(repo, dto.Id, true);
        if (category == null) return new GoodResponse(new CategoryNotExistsDto());

        // Check name duplication
        if (dto.Name != null && category.Name != dto.Name)
            if (await CategoryUtil.HasCategoryOfUserAsync(repo, id, dto.Name))
                return new GoodResponse(new CategoryAlreadyExistsDto());

        // Update whole category.
        CategoryUtil.UpdateCategory(category, dto);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
        }

        CategoryDto? data = _mapper.Map<Category, CategoryDto>(category);

        return new GoodResponse(new GoodDto("Category updated", data));
    }


    public async Task<ApiResponse> MergeCategory(int id, MergeCategoryDto dto)
    {
        if (!dto.Format().Verify()) return new BadRequestResponse(new BadRequestDto());

        if (dto.SrcId == dto.DstId) return new BadRequestResponse(new CategoryMergeSelfErrorDto());

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null) return new GoodResponse(new UserNotExistsDto());

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        Category? src;
        Category? dst;
        try
        {
            src = dto.SrcId == 0 ? null : await Category.GetAsync(repo, dto.SrcId);
            dst = dto.DstId == 0 ? null : await Category.GetAsync(repo, dto.DstId);
        }
        catch
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        await CategoryUtil.MergeCategoriesAsync(_unitOfWork, src, dst);
        if (dto.Delete && src != null) await CategoryUtil.EraseCategoryAsync(_unitOfWork, src, user);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
        }

        return new GoodResponse(new GoodDto("Categories Merged"));
    }


    public async Task<ApiResponse> GetCategories(int id, bool authorized)
    {
        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null) return new GoodResponse(new UserNotExistsDto());

        IRepository<Category> repo = _unitOfWork.GetRepository<Category>();
        IList<Category> categoryList = await repo.GetAllAsync(
            predicate: x => x.UserId == id,
            include: source => source.Include(x => x.Option),
            orderBy: source => source.OrderBy(x => x.Name));

        var data = new GetCategoryDto();
        foreach (Category category in categoryList)
        {
            // skip private category
            if (!authorized && !category.Option.IsPublic) continue;

            data.Categories.Add(_mapper.Map<Category, CategoryDto>(category));
        }

        return new GoodResponse(new GoodWithDataDto(data));
    }
}