using System.Globalization;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeGlobal;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using BadgeBoard.Api.Modules.BadgeUser.Services.Utils;
using static System.Int32;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class UserService : BaseService, IUserService
	{
		public UserService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public async Task<ApiResponse> Exists(string type, string value)
		{
			if (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(value)) {
				return new GoodResponse(new GoodWithDataDto(false));
			}

			var repo = _unitOfWork.GetRepository<User>();
			bool data;
			switch (type) {
				case "id":
					if (TryParse(value, out var id)) {
						data = await UserUtil.HasUserByIdAsync(repo, id);
					} else {
						return new BadRequestResponse(new BadRequestDto("Bad ID"));
					}
					break;
				case "username":
					data = await UserUtil.HasUserByUsernameAsync(repo, value);
					break;
				default:
					return new BadRequestResponse(new BadRequestDto("Bad type"));
			};

			return new GoodResponse(new GoodWithDataDto(data));
		}

		public async Task<ApiResponse> UpdatePreference(int id, UpdateUserPreferenceDto dto)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			user.Preference = await UserPreference.GetAsync(_unitOfWork.GetRepository<UserPreference>(), user.UserPreferenceId);
			user.Preference.IsDefaultPublic = dto.IsDefaultPublic ?? user.Preference.IsDefaultPublic;
			repo.Update(user);
			await _unitOfWork.SaveChangesAsync();

			return new GoodResponse(new GoodWithDataDto(_mapper.Map<UserPreference, UserPreferenceDto>(user.Preference)));
		}

		public async Task<ApiResponse> UpdateInfo(int id, UpdateUserInfoDto dto)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			user.Info = await UserInfo.GetAsync(_unitOfWork.GetRepository<UserInfo>(), user.UserInfoId);
			user.Info.Motto = dto.Motto ?? user.Info.Motto;
			if (dto.Birthday != null) {
				// silent on failure
				try {
					var birthday = DateTime.ParseExact(dto.Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
					if (birthday < DateTime.Today) {
						user.Info.Birthday = birthday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
					}
				} catch (FormatException) {
				}
			}

			if (dto.Sex != null) {
				if (UserSex.IsValid((int)dto.Sex)) {
					user.Info.Sex = dto.Sex;
				}
			}

			repo.Update(user);
			await _unitOfWork.SaveChangesAsync();

			return new GoodResponse(new GoodWithDataDto(_mapper.Map<UserInfo, UserInfoDto>(user.Info)));
		}

		public async Task<ApiResponse> UpdateUsername(int id, UpdateUsernameDto dto)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			if (!AccountVerifier.VerifyUsername(dto.Username)) {
				return new BadRequestResponse(new BadRequestDto("Bad username"));
			}

			if (user.Username == dto.Username) {
				return new GoodResponse(new GoodWithDataDto(user.Username));
			}

			if (await UserUtil.HasUserByUsernameAsync(repo, dto.Username)) {
				return new GoodResponse(new UserAlreadyExistsDto("Duplicated username"));
			}

			user.Username = dto.Username ?? user.Username;
			await _unitOfWork.SaveChangesAsync();

			return new GoodResponse(new GoodWithDataDto(user.Username));
		}

		public async Task<ApiResponse> UpdateAvatar(int id, UpdateAvatarDto dto)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			if (!AvatarUtil.DeleteAvatar(user.AvatarUrl)) {
				return new GoodResponse(new BadDto(Errors.DeleteAvatarError, "Failed to delete old avatar"));
			}

			var avatarUrl = AvatarUtil.SaveAvatar(dto.Data, dto.Extension);
			if (avatarUrl == null) {
				return new GoodResponse(new BadDto(Errors.SaveAvatarError, "Failed to save new avatar"));
			}

			user.AvatarUrl = avatarUrl;
			await _unitOfWork.SaveChangesAsync();

			return new GoodResponse(new GoodDto("Nice avatar!", user.AvatarUrl));
		}

		public async Task<ApiResponse> GetUser(int id)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			await User.IncludeAsync(_unitOfWork, user);
			var data = _mapper.Map<User, UserGeneralDto>(user);

			return new GoodResponse(new GoodWithDataDto(data));
		}

		public async Task<ApiResponse> GetCurrentUser(int id)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await UserUtil.GetUserByIdAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			await User.IncludeAsync(_unitOfWork, user);
			var data = _mapper.Map<User, UserCompleteDto>(user);

			return new GoodResponse(new GoodWithDataDto(data));
		}
	}
}
