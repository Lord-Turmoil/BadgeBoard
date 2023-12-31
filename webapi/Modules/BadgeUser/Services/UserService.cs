﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.Globalization;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;
using static System.Int32;

namespace BadgeBoard.Api.Modules.BadgeUser.Services;

public class UserService : BaseService, IUserService
{
    public UserService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork,
        mapper) { }


    public async Task<ApiResponse> Exists(string type, string value)
    {
        if (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(value))
        {
            return new GoodResponse(new GoodWithDataDto(false));
        }

        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        bool data;
        switch (type)
        {
            case "id":
                if (TryParse(value, out var id))
                {
                    data = await UserUtil.HasUserByIdAsync(repo, id);
                }
                else
                {
                    return new BadRequestResponse(new BadRequestDto("Bad ID"));
                }

                break;
            case "username":
                data = await UserUtil.HasUserByUsernameAsync(repo, value);
                break;
            default:
                return new BadRequestResponse(new BadRequestDto("Bad type"));
        }

        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> UpdatePreference(int id, UpdateUserPreferenceDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        try
        {
            user.Preference = await UserPreference.GetAsync(_unitOfWork.GetRepository<UserPreference>(),
                user.UserPreferenceId);
            user.Preference.IsDefaultPublic = dto.IsDefaultPublic ?? user.Preference.IsDefaultPublic;
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(data: ex));
        }

        return new GoodResponse(new GoodWithDataDto(_mapper.Map<UserPreference, UserPreferenceDto>(user.Preference)));
    }


    public async Task<ApiResponse> UpdateInfo(int id, UpdateUserInfoDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        try
        {
            user.Info = await UserInfo.GetAsync(_unitOfWork.GetRepository<UserInfo>(), user.UserInfoId);
        }
        catch (MissingReferenceException ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(data: ex));
        }

        user.Info.Motto = dto.Motto ?? user.Info.Motto;
        if (dto.Birthday != null)
            // silent on failure
        {
            try
            {
                DateTime birthday = DateTime.ParseExact(dto.Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (birthday < DateTime.Today)
                {
                    user.Info.Birthday = birthday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
            }
            catch (FormatException) { }
        }

        if (dto.Sex != null)
        {
            if (UserSex.IsValid((int)dto.Sex))
            {
                user.Info.Sex = dto.Sex;
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return new GoodResponse(new GoodWithDataDto(_mapper.Map<UserInfo, UserInfoDto>(user.Info)));
    }


    public async Task<ApiResponse> UpdateUsername(int id, UpdateUsernameDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        if (dto.Username == null || !AccountVerifier.VerifyUsername(dto.Username))
        {
            return new BadRequestResponse(new BadRequestDto("Bad username"));
        }

        if (user.Username == dto.Username)
        {
            return new GoodResponse(new GoodWithDataDto(user.Username));
        }

        // dto.Username can't be null here
        if (await UserUtil.HasUserByUsernameAsync(repo, dto.Username))
        {
            return new GoodResponse(new UserAlreadyExistsDto("Duplicated username"));
        }

        user.Username = dto.Username;
        await _unitOfWork.SaveChangesAsync();

        return new GoodResponse(new GoodWithDataDto(user.Username));
    }


    public async Task<ApiResponse> UpdateAvatar(int id, UpdateAvatarDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        if (!AvatarUtil.DeleteAvatar(user.AvatarUrl))
        {
            return new GoodResponse(new BadDto(Errors.DeleteAvatarError, "Failed to delete old avatar"));
        }

        var avatarUrl = AvatarUtil.SaveAvatar(dto.Data, dto.Extension);
        if (avatarUrl == null)
        {
            return new GoodResponse(new BadDto(Errors.SaveAvatarError, "Failed to save new avatar"));
        }

        user.AvatarUrl = avatarUrl;

        await _unitOfWork.SaveChangesAsync();
        return new GoodResponse(new GoodDto("Nice avatar!", user.AvatarUrl));
    }


    public async Task<ApiResponse> GetUser(int id)
    {
        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        await User.IncludeAsync(_unitOfWork, user);
        UserGeneralDto? data = _mapper.Map<User, UserGeneralDto>(user);

        return new GoodResponse(new GoodWithDataDto(data));
    }


    public async Task<ApiResponse> GetCurrentUser(int id)
    {
        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await User.FindAsync(repo, id);
        if (user == null)
        {
            return new GoodResponse(new UserNotExistsDto());
        }

        try
        {
            await User.IncludeAsync(_unitOfWork, user);
        }
        catch (MissingReferenceException ex)
        {
            return new InternalServerErrorResponse(new MissingReferenceDto(data: ex));
        }

        UserCompleteDto? data = _mapper.Map<User, UserCompleteDto>(user);

        return new GoodResponse(new GoodWithDataDto(data));
    }
}