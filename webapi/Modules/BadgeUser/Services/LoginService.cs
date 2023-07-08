using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;
using Microsoft.Extensions.Options;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class LoginService : BaseService, ILoginService
	{
		public LoginService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider,
			unitOfWork, mapper)
		{
		}

		public async Task<ApiResponse> Login(LoginDto dto)
		{
			if (!dto.Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByUsernameAsync(repo, dto.Username);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			await User.IncludeAsync(_unitOfWork, user);

			if (!AccountUtil.VerifyPasswordHash(dto.Password, user.Account.Salt, user.Account.Password)) {
				return new GoodResponse(new LoginWrongPasswordDto());
			}

			var options = _provider.GetRequiredService<IOptions<JwtOptions>>();
			var jwt = JwtUtil.CreateToken(options, user.Id.ToString());
			var userDto = _mapper.Map<User, UserDto>(user);

			return new GoodResponse(new LoginSuccessDto(userDto, jwt));
		}

		public async Task<TokenResponseData> GetToken(TokenDto dto)
		{
			if (!dto.Verify()) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Status = StatusCodes.Status400BadRequest,
					Message = "Request format error"
				};
			}

			// verify user existence
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, dto.Id);
			if (user == null) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Message = "No such user"
				};
			}

			// verify password
			user.Account = await UserAccount.GetAsync(_unitOfWork.GetRepository<UserAccount>(), user.Id);
			if (!AccountUtil.VerifyPasswordHash(dto.Password, user.Account.Salt, user.Account.Password)) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Message = "Wrong password"
				};
			}

			// verification complete
			var data = new TokenResponseData {
				Id = user.Id,
				IsAuthenticated = true
			};

			var options = _provider.GetRequiredService<IOptions<JwtOptions>>();
			data.Token = JwtUtil.CreateToken(options, user.Id.ToString());
			if (user.RefreshTokens.Any(t => t.IsActive)) {
				var activeToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive);
				data.RefreshToken = activeToken.Token;
				data.RefreshTokenExpiration = activeToken.Expires;
			} else {
				var refreshToken = TokenUtil.CreateRefreshToken();
				data.RefreshToken = refreshToken.Token;
				data.RefreshTokenExpiration = refreshToken.Expires;
				user.RefreshTokens.Add(refreshToken);
				repo.Update(user);
				await _unitOfWork.SaveChangesAsync();
			}

			return data;
		}
	}
}
