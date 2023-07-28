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

namespace BadgeBoard.Api.Modules.BadgeBadge.Services;

public class BrowseService : BaseService, IBrowseService
{
    public BrowseService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
        : base(provider, unitOfWork, mapper) { }


    public async Task<ApiResponse> GetBadge(int badgeId)
    {
        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), badgeId);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (!badge.IsPublic)
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

        if (user.IsAdmin || badge.IsPublic || badge.UserId == user.Id)
        {
            BadgeDto dto = await BadgeDtoUtil.GetBadgeDtoAsync(_unitOfWork, _mapper, badge);
            return new GoodResponse(new GoodWithDataDto(dto));
        }

        return new GoodResponse(new BadgeIsPrivateDto());
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

        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(x => x.IsPublic && x.UserId == dto.UserId);
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
        if (initiator == user || initiator.IsAdmin)
        {
            predicate = x => x.UserId == dto.UserId;
        }
        else
        {
            predicate = x => x.IsPublic && x.UserId == dto.UserId;
        }

        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(predicate);
        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetBadgesOfCategory(BrowseCategoryBadgeDto dto)
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

        Category? category = null;
        if (dto.CategoryId != 0)
        {
            category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.CategoryId, true);
            if (category == null)
            {
                return new GoodResponse(new CategoryNotExistsDto());
            }
        }

        if (category != null)
        {
            if (category.UserId != user.Id)
            {
                return new GoodResponse(new CategoryNotMatchUserDto());
            }

            if (!category.Option.IsPublic)
            {
                return new GoodResponse(new CategoryIsPrivateDto());
            }
        }

        // here, it is obvious that badges of category belong to the user
        var dbCategoryId = category?.Id;
        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(x => x.CategoryId == dbCategoryId);
        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetBadgesOfCategory(int id, BrowseCategoryBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        User? initiator = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
        if (initiator == null)
        {
            return new GoodResponse(new UserNotExistsDto("Initiator not exist"));
        }

        User? user = dto.UserId == id ? initiator : await User.FindAsync(_unitOfWork.GetRepository<User>(), dto.UserId);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        Category? category = null;


        var dbCategoryId = category?.Id;
        Expression<Func<Badge, bool>> predicate;
        if (initiator.IsAdmin || category == null || category.UserId == initiator.Id)
        {
            predicate = x => x.CategoryId == dbCategoryId;
        }
        else
        {
            predicate = x => x.CategoryId == dbCategoryId && x.IsPublic;
        }

        BrowseBadgeSuccessDto data = await _GetBadgeDtosData(predicate);

        return new GoodResponse(new GoodWithDataDto(data));
    }


    private async Task<BrowseBadgeSuccessDto> _GetBadgeDtosData(Expression<Func<Badge, bool>> predicate)
    {
        IList<Badge> badges = await _unitOfWork.GetRepository<Badge>().GetAllAsync(
            predicate, source => source.OrderBy(x => x.CreatedTime));
        IList<BadgeDto> badgeDtos = await BadgeDtoUtil.GetBadgeDtosAsync(_unitOfWork, _mapper, badges);
        return new BrowseBadgeSuccessDto {
            Count = badgeDtos.Count,
            Badges = badgeDtos
        };
    }
}