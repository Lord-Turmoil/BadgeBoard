// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Arch.EntityFrameworkCore.UnitOfWork;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services;

namespace BadgeBoard.Api.Modules.BadgeUser;

public class UserModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddCustomRepository<User, UserRepository>()
            .AddCustomRepository<UserPreference, UserPreferenceRepository>()
            .AddCustomRepository<UserInfo, UserInfoRepository>()
            .AddCustomRepository<FavoriteUser, FavoriteUserRepository>();

        services.AddScoped<ILoginService, LoginService>()
            .AddScoped<IRegisterService, RegisterService>()
            .AddScoped<IUserService, UserService>();

        return services;
    }
}