// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Linq.Expressions;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services;

public class BrowseService : BaseService, IBrowseService
{
    public BrowseService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
        : base(provider, unitOfWork, mapper) { }


    public async Task<ApiResponse> GetBadge(int badgeId)
    {
        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), badgeId, true);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (!BadgeUtil.IsAccessible(badge))
        {
            return new GoodResponse(new BadgeIsPrivateDto());
        }

        BadgeDto dto = await BadgeDtoUtil.GetBadgeDtoAsync(_unitOfWork, _mapper, badge);

        return new GoodResponse(new GoodWithDataDto(dto));
    }


    public async Task<ApiResponse> GetBadge(int id, int badgeId)
    {
        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), badgeId);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (!BadgeUtil.IsAccessible(badge, user))
        {
            return new GoodResponse(new BadgeIsPrivateDto());
        }

        BadgeDto dto = await BadgeDtoUtil.GetBadgeDtoAsync(_unitOfWork, _mapper, badge);
        return new GoodResponse(new GoodWithDataDto(dto));
    }


    public async Task<ApiResponse> GetBadgesOfUser(BrowseAllBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), dto.UserId);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(x => x.IsPublic && x.UserId == dto.UserId, true);
        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetBadgesOfUser(int id, BrowseAllBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? initiator = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (initiator == null)
        {
            return new GoodResponse(new UserNotExistsDto("Initiator not exists"));
        }

        User? user = dto.UserId == id ? initiator : await User.FindAsync(_unitOfWork.GetRepository<User>(), dto.UserId);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        Expression<Func<Badge, bool>> predicate;
        bool publicCategoryOnly;
        if (initiator == user || initiator.IsAdmin)
        {
            predicate = x => x.UserId == dto.UserId;
            publicCategoryOnly = false;
        }
        else
        {
            predicate = x => x.UserId == dto.UserId && x.IsPublic;
            publicCategoryOnly = true;
        }

        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(predicate, publicCategoryOnly);
        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetBadgesOfCategory(BrowseCategoryBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        Category? category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.CategoryId, true);
        if (category == null)
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        if (!CategoryUtil.IsAccessible(category))
        {
            return new GoodResponse(new CategoryIsPrivateDto());
        }

        // here, it is obvious that badges of category belong to the user,
        // and visibility of category is already checked.
        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(x => x.CategoryId == category.Id, false);
        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetBadgesOfCategory(int id, BrowseCategoryBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto("Initiator not exist"));
        }

        Category? category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.CategoryId, true);
        if (category == null)
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        if (!CategoryUtil.IsAccessible(category, user))
        {
            return new GoodResponse(new CategoryIsPrivateDto());
        }

        Expression<Func<Badge, bool>> predicate;
        if (user.IsAdmin || category.UserId == user.Id)
        {
            predicate = x => x.CategoryId == category.Id;
        }
        else
        {
            predicate = x => x.CategoryId == category.Id && x.IsPublic;
        }

        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(predicate, false);

        return new GoodResponse(new GoodWithDataDto(data));
    }


    private async Task<BrowseBadgeSuccessDto> _GetBadgeDtosData(Expression<Func<Badge, bool>> predicate, bool publicCategoryOnly)
    {
        IEnumerable<Badge> badges = await _unitOfWork.GetRepository<Badge>().GetAllAsync(
            predicate: predicate,
            orderBy: source => source.OrderBy(x => x.CreatedTime),
            include: source => source.Include(x => x.Category).ThenInclude(x => x.Option));
        if (publicCategoryOnly)
        {
            badges = badges.Where(x => x.Category.Option.IsPublic);
        }

        IList<BadgeDto> badgeDtos = await BadgeDtoUtil.GetBadgeDtosAsync(_unitOfWork, _mapper, badges);
        return new BrowseBadgeSuccessDto {
            Count = badgeDtos.Count,
            Badges = badgeDtos
        };
    }
}