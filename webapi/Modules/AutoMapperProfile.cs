// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using AutoMapper;
using BadgeBoard.Api.Modules.BadgeAccount.Dtos;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Category;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Unread;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules;

public class AutoMapperProfile : MapperConfigurationExpression
{
	public AutoMapperProfile()
	{
		// Account Module
		CreateMap<UserAccount, UserAccountDto>();

		// User Module
		CreateMap<User, UserLoginDto>().ReverseMap();
		CreateMap<User, UserBriefDto>().ReverseMap();
		CreateMap<User, UserGeneralDto>().ReverseMap();
		CreateMap<User, UserCompleteDto>().ReverseMap();
		CreateMap<UserPreference, UserPreferenceDto>().ReverseMap();
		CreateMap<UserInfo, UserInfoDto>().ReverseMap();

		// Badge Module
		CreateMap<Category, CategoryDto>().ReverseMap();
		CreateMap<Category, CategoryBriefDto>().ReverseMap();
		CreateMap<CategoryOption, CategoryOptionDto>().ReverseMap();

		CreateMap<Badge, BadgeDto>().ReverseMap();
		CreateMap<QuestionPayload, QuestionPayloadDto>().ReverseMap();
		CreateMap<MemoryPayload, MemoryPayloadDto>().ReverseMap();

		CreateMap<UnreadRecord, UnreadRecordDto>().ReverseMap();
	}
}