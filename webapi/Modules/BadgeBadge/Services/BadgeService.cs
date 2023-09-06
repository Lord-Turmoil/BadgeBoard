// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services;

public class BadgeService : BaseService, IBadgeService
{
    public BadgeService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper)
        : base(provider, unitOfWork, mapper) { }


    /// <summary>
    ///     dto.SrcId == 0 means anonymous, and id == 0 means no authorization
    ///     provided. If not anonymous, must be consistent with authorized id.
    ///     In anonymous request, both dto.SrcId and id will be 0.
    ///     In non-anonymous request, both dto.SrcId and id will not be 0, and
    ///     must be consistent.
    ///     This prerequisite is guaranteed by controller.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<ApiResponse> AddQuestionBadge(int id, AddQuestionBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        if (dto.Type != Badge.Types.Question)
        {
            return new BadRequestResponse(new BadRequestDto("Wrong badge type"));
        }

        if (dto.SrcId != id)
        {
            return new BadRequestResponse(new BadRequestDto("Sender inconsistent"));
        }

        // get category
        Category? category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.Category, true);
        if (category == null)
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        if (!category.Option.IsPublic && category.UserId != id)
        {
            return new GoodResponse(new CategoryIsPrivateDto());
        }

        if (!category.Option.AllowQuestion)
        {
            return new GoodResponse(new BadgeTypeNotAllowedDto(BadgeTypeNotAllowedDto.QuestionNotAllowedMessage));
        }

        // get sender and receiver
        IRepository<User> userRepo = _unitOfWork.GetRepository<User>();
        User? sender = null;
        if (dto.SrcId != 0)
        {
            sender = await User.FindAsync(userRepo, dto.SrcId);
            if (sender == null)
            {
                return new GoodResponse(new UserNotExistsDto());
            }
        }

        if (sender == null && category is { Option.AllowAnonymity: false })
        {
            return new GoodResponse(new AnonymousBadgeNotAllowed());
        }

        User receiver;
        try
        {
            receiver = await User.GetAsync(userRepo, dto.DstId);
        }
        catch
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        // create new badge
        QuestionBadgePack pack = await BadgeUtil.AddQuestionBadgeAsync(_unitOfWork, dto, sender, receiver, category);

        // add unread record
        try
        {
            UnreadRecord unread =
                await UnreadRecord.GetAsync(_unitOfWork.GetRepository<UnreadRecord>(), receiver.UnreadRecordId);
            unread.QuestionCount++;
        }
        catch (MissingReferenceException ex)
        {
            return new InternalServerErrorResponse(new MissingReferenceDto(data: ex));
        }

        // save changes
        await _unitOfWork.SaveChangesAsync();

        // return single badge dto
        try
        {
            BadgeDto data = await BadgeDtoUtil.GetQuestionBadgeDtoAsync(_unitOfWork, _mapper, pack);
            return new GoodResponse(new GoodDto("Question badge placed", data));
        }
        catch (Exception ex)
        {
            return new GoodResponse(new OrdinaryDto(Errors.FailedToGetBadge, ex.Message));
        }
    }


    public async Task<ApiResponse> AddMemoryBadge(int id, AddMemoryBadgeDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        if (dto.Type != Badge.Types.Memory)
        {
            return new BadRequestResponse(new BadRequestDto("Wrong badge type"));
        }

        if (dto.SrcId != id)
        {
            return new BadRequestResponse(new BadRequestDto("Sender inconsistent"));
        }

        // get category
        Category? category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.Category, true);
        if (category == null)
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        if (!category.Option.IsPublic && category.UserId != id)
        {
            return new GoodResponse(new CategoryIsPrivateDto());
        }

        if (!category.Option.AllowMemory)
        {
            return new GoodResponse(new BadgeTypeNotAllowedDto(BadgeTypeNotAllowedDto.MemoryNotAllowedMessage));
        }

        // get sender and receiver
        IRepository<User> userRepo = _unitOfWork.GetRepository<User>();
        User? sender = null;
        if (dto.SrcId != 0)
        {
            sender = await User.FindAsync(userRepo, dto.SrcId);
            if (sender == null)
            {
                return new GoodResponse(new UserNotExistsDto());
            }
        }

        if (sender == null && category is { Option.AllowAnonymity: false })
        {
            return new GoodResponse(new AnonymousBadgeNotAllowed());
        }

        User receiver;
        try
        {
            receiver = await User.GetAsync(userRepo, dto.DstId);
        }
        catch
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        // create new badge
        MemoryBadgePack pack = await BadgeUtil.AddMemoryBadgeAsync(_unitOfWork, dto, sender, receiver, category);

        // add record if not sending for self
        if (!(sender != null && sender.Id == receiver.Id))
        {
            try
            {
                UnreadRecord unread =
                    await UnreadRecord.GetAsync(_unitOfWork.GetRepository<UnreadRecord>(), receiver.UnreadRecordId);
                unread.MemoryCount++;
            }
            catch (MissingReferenceException ex)
            {
                return new InternalServerErrorResponse(new MissingReferenceDto(data: ex));
            }
        }

        await _unitOfWork.SaveChangesAsync();

        // return single badge dto
        try
        {
            BadgeDto data = await BadgeDtoUtil.GetMemoryBadgeDtoAsync(_unitOfWork, _mapper, pack);
            return new GoodResponse(new GoodDto("Memory badge placed", data));
        }
        catch (Exception ex)
        {
            return new GoodResponse(new OrdinaryDto(Errors.FailedToGetBadge, ex.Message));
        }
    }


    public async Task<ApiResponse> DeleteBadge(int id, DeleteBadgeDto dto)
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

        var data = new DeleteBadgeSuccessDto();
        data.Errors.AddRange(await BadgeUtil.EraseBadgesAsync(_unitOfWork, dto.Badges, user, dto.Force));

        await _unitOfWork.SaveChangesAsync();

        var message = data.Errors.Count == 0 ? "Deletion succeeded" : "Deletion partial succeeded";

        return new GoodResponse(new GoodDto(message, data));
    }


    public async Task<ApiResponse> UpdateBadge(int id, UpdateBadgeDto dto)
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

        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (badge.UserId != user.Id)
        {
            return new GoodResponse(new NotYourBadgeDto());
        }

        if (dto.Style != null)
        {
            badge.Style = dto.Style;
        }

        if (dto.IsPublic != null)
        {
            badge.IsPublic = (bool)dto.IsPublic;
        }

        await _unitOfWork.SaveChangesAsync();
        var data = new UpdateBadgeSuccessDto {
            IsPublic = badge.IsPublic,
            Style = badge.Style
        };
        return new GoodResponse(new GoodDto("Badge updated", data));
    }


    public async Task<ApiResponse> UpdateQuestionBadge(int id, UpdateQuestionBadgeDto dto)
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

        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (badge.Type != Badge.Types.Question)
        {
            return new GoodResponse(new WrongBadgeTypeDto());
        }

        if (badge.UserId != user.Id)
        {
            return new GoodResponse(new NotYourBadgeDto());
        }

        QuestionPayload? payload = await QuestionPayload.FindAsync(
            _unitOfWork.GetRepository<QuestionPayload>(), badge.PayloadId);
        if (payload == null)
        {
            return new GoodResponse(new PayloadNotExistsDto());
        }

        payload.Answer = dto.Answer;
        if (!badge.IsChecked && payload.Answer != null)
        {
            badge.IsChecked = true;
            try
            {
                UnreadRecord unread =
                    await UnreadRecord.GetAsync(_unitOfWork.GetRepository<UnreadRecord>(), user.UnreadRecordId);
                if (unread.QuestionCount > 0)
                {
                    unread.QuestionCount--;
                }
            }
            catch (MissingReferenceException ex)
            {
                return new InternalServerErrorResponse(new MissingReferenceDto(data: ex));
            }
        }

        await _unitOfWork.SaveChangesAsync();

        var data = new UpdateQuestionBadgeSuccessDto {
            Answer = payload.Answer
        };
        return new GoodResponse(new GoodDto("Question Badge updated", data));
    }


    public async Task<ApiResponse> UpdateMemoryBadge(int id, UpdateMemoryBadgeDto dto)
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

        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (badge.Type != Badge.Types.Memory)
        {
            return new GoodResponse(new WrongBadgeTypeDto());
        }

        if (badge.UserId != user.Id)
        {
            return new GoodResponse(new NotYourBadgeDto());
        }

        MemoryPayload? payload = await MemoryPayload.FindAsync(
            _unitOfWork.GetRepository<MemoryPayload>(), badge.PayloadId);
        if (payload == null)
        {
            return new GoodResponse(new PayloadNotExistsDto());
        }

        payload.Memory = dto.Memory;

        await _unitOfWork.SaveChangesAsync();

        var data = new UpdateMemoryBadgeSuccessDto {
            Memory = payload.Memory
        };
        return new GoodResponse(new GoodDto("Memory Badge updated", data));
    }


    public async Task<ApiResponse> MoveBadge(int id, MoveBadgeDto dto)
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

        Badge? badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
        if (badge == null)
        {
            return new GoodResponse(new BadgeNotExistsDto());
        }

        if (badge.UserId != user.Id)
        {
            return new GoodResponse(new NotYourBadgeDto());
        }

        // Well, hope front end will handle requests that moves to itself.
        Category? category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.Category);
        if (category == null)
        {
            return new GoodResponse(new CategoryNotExistsDto());
        }

        badge.CategoryId = category.Id;

        await _unitOfWork.SaveChangesAsync();

        return new GoodResponse(new GoodDto($"Badge moved to {category.Name}"));
    }
}