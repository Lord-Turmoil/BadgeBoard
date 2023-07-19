using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Jwt;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
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
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.FindUserByUsernameAsync(repo, dto.Username);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			try {
				await User.IncludeAsync(_unitOfWork, user);
			} catch (MissingReferenceException ex) {
				return new InternalServerErrorResponse(new InternalServerErrorDto(data: ex));
			}

			if (!AccountUtil.VerifyPasswordHash(dto.Password, user.Account.Salt, user.Account.Password)) {
				return new GoodResponse(new LoginWrongPasswordDto());
			}

			var userDto = _mapper.Map<User, UserCompleteDto>(user);

			return new GoodResponse(new GoodDto("Welcome back, my friend", userDto));
		}

		public async Task<TokenResponseData> GetToken(TokenDto dto)
		{
			if (!dto.Format().Verify()) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Status = StatusCodes.Status400BadRequest,
					Message = "Request format error"
				};
			}

			// verify user existence
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.FindUserByIdAsync(repo, dto.Id);
			if (user == null) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Message = "No such user"
				};
			}

			// verify password
			try {
				user.Account = await UserAccount.GetAsync(_unitOfWork.GetRepository<UserAccount>(), user.Id);
			} catch {
				return new TokenResponseData {
					Status = StatusCodes.Status500InternalServerError,
					IsAuthenticated = false,
					Message = "Missing Account reference"
				};
			}

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
			var activeToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive);
			if (activeToken != null) {
				data.RefreshToken = activeToken.Token;
				data.RefreshTokenExpiration = activeToken.Expires;
			} else {
				var refreshToken = TokenUtil.CreateRefreshToken();
				data.RefreshToken = refreshToken.Token;
				data.RefreshTokenExpiration = refreshToken.Expires;
				user.RefreshTokens.Add(refreshToken);
				try {
					await _unitOfWork.SaveChangesAsync();
				} catch {
					return new TokenResponseData {
						Status = StatusCodes.Status500InternalServerError,
						IsAuthenticated = false,
						Message = "Failed to save data"
					};
				}
			}

			return data;
		}

		public async Task<TokenResponseData> RefreshToken(string oldToken)
		{
			if (string.IsNullOrWhiteSpace(oldToken)) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Status = StatusCodes.Status400BadRequest,
					Message = "Invalid oldToken"
				};
			}

			var repo = _unitOfWork.GetRepository<User>();

			// Get oldToken owner
			var user = await repo.GetFirstOrDefaultAsync(predicate: x => x.RefreshTokens.Any(t => t.Token == oldToken));
			if (user == null) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Message = "Token did not match any users"
				};
			}

			// Get refresh oldToken
			var refreshToken = user.RefreshTokens.Single(x => x.Token == oldToken);
			if (!refreshToken.IsActive) {
				return new TokenResponseData {
					IsAuthenticated = false,
					Message = "Token not active"
				};
			}

			// Revoke current refresh oldToken
			refreshToken.Revoked = DateTime.UtcNow;

			// Generate a new refresh oldToken
			var token = TokenUtil.CreateRefreshToken();
			user.RefreshTokens.Add(token);
			try {
				await _unitOfWork.SaveChangesAsync();
			} catch {
				return new TokenResponseData {
					Status = StatusCodes.Status500InternalServerError,
					IsAuthenticated = false,
					Message = "Failed to save data"
				};
			}

			// Generate new JWT
			var data = new TokenResponseData {
				Id = user.Id,
				IsAuthenticated = true
			};

			var options = _provider.GetRequiredService<IOptions<JwtOptions>>();
			data.Token = JwtUtil.CreateToken(options, user.Id.ToString());
			data.RefreshToken = token.Token;
			data.RefreshTokenExpiration = token.Expires;

			return data;
		}

		public async Task<RevokeTokenData> RevokeToken(string oldToken)
		{
			if (string.IsNullOrWhiteSpace(oldToken)) {
				return new RevokeTokenData {
					Succeeded = false,
					Message = "Invalid oldToken"
				};
			}

			var repo = _unitOfWork.GetRepository<User>();

			var user = await repo.GetFirstOrDefaultAsync(
				predicate: x => x.RefreshTokens.Any(t => t.Token == oldToken));
			if (user == null) {
				return new RevokeTokenData {
					Succeeded = false,
					Message = "Token did not match any users"
				};
			}

			var token = user.RefreshTokens.Single(t => t.Token == oldToken);
			if (!token.IsActive) {
				return new RevokeTokenData {
					Succeeded = true,
					Message = "Token not active already"
				};
			}

			token.Revoked = DateTime.UtcNow;
			try {
				await _unitOfWork.SaveChangesAsync();
			} catch {
				return new RevokeTokenData {
					Status = StatusCodes.Status500InternalServerError,
					Succeeded = false,
					Message = "Failed to save data"
				};
			}

			return new RevokeTokenData {
				Succeeded = true,
				Message = "Token revoked"
			};
		}
	}
}
