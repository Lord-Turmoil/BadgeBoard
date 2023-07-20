﻿using AutoMapper;
using BadgeBoard.Api.Modules.BadgeAccount.Dtos;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules
{
	public class AutoMapperProfile : MapperConfigurationExpression
	{
		public AutoMapperProfile()
		{
			// Account Module
			CreateMap<UserAccount, UserAccountDto>();

			// User Module
			CreateMap<User, UserCompleteDto>().ReverseMap();
			CreateMap<User, UserGeneralDto>().ReverseMap();
			CreateMap<UserPreference, UserPreferenceDto>().ReverseMap();
			CreateMap<UserInfo, UserInfoDto>().ReverseMap();

			// Badge Module
			CreateMap<Category, CategoryDto>().ReverseMap();
			CreateMap<CategoryOption, CategoryOptionDto>().ReverseMap();
		}
	}
}
