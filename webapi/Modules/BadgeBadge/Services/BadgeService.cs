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
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services
{
	public class BadgeService : BaseService, IBadgeService
	{
		protected BadgeService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		/// <summary>
		/// dto.SrcId == 0 means anonymous, and id == 0 means no authorization
		/// provided. If not anonymous, must be consistent with authorized id.
		/// In anonymous request, both dto.SrcId and id will be 0.
		/// In non-anonymous request, both dto.SrcId and id will not be 0, and
		/// must be consistent.
		/// This prerequisite is guaranteed by controller. 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="dto"></param>
		/// <returns></returns>
		public async Task<ApiResponse> AddQuestionBadge(int id, AddQuestionBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			if (dto.Type != Badge.Types.Question) {
				return new BadRequestResponse(new BadRequestDto("Wrong badge type"));
			}

			if (dto.SrcId != id) {
				return new BadRequestResponse(new BadRequestDto("Sender inconsistent"));
			}

			// get sender and receiver
			var userRepo = _unitOfWork.GetRepository<User>();
			User? sender = null;
			if (dto.SrcId != 0) {
				sender = await User.FindAsync(userRepo, dto.SrcId);
				if (sender == null) {
					return new GoodResponse(new UserNotExistsDto());
				}
			}
			User receiver;
			try {
				receiver = await User.GetAsync(userRepo, dto.DstId);
			} catch {
				return new GoodResponse(new UserNotExistsDto());
			}

			// get category
			Category? category = null;
			if (dto.Category != 0) {
				category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.Category);
				if (category == null) {
					return new GoodResponse(new CategoryNotExistsDto());
				}
			}

			// create new badge
			var pack = await BadgeUtil.AddQuestionBadgeAsync(_unitOfWork, dto, sender, receiver, category);
			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			// return single badge dto
			try {
				var data = await BadgeDtoUtil.GetQuestionBadgeDtoAsync(_unitOfWork, _mapper, pack);
				return new GoodResponse(new GoodDto("Question badge placed", data));
			} catch (Exception ex) {
				return new GoodResponse(new OrdinaryDto(Errors.FailedToGetBadge, ex.Message));
			}
		}

		public async Task<ApiResponse> AddMemoryBadge(int id, AddMemoryBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			if (dto.Type != Badge.Types.Memory) {
				return new BadRequestResponse(new BadRequestDto("Wrong badge type"));
			}

			if (dto.SrcId != id) {
				return new BadRequestResponse(new BadRequestDto("Sender inconsistent"));
			}

			// get sender and receiver
			var userRepo = _unitOfWork.GetRepository<User>();
			User? sender = null;
			if (dto.SrcId != 0) {
				sender = await User.FindAsync(userRepo, dto.SrcId);
				if (sender == null) {
					return new GoodResponse(new UserNotExistsDto());
				}
			}
			User receiver;
			try {
				receiver = await User.GetAsync(userRepo, dto.DstId);
			} catch {
				return new GoodResponse(new UserNotExistsDto());
			}

			// get category
			Category? category = null;
			if (dto.Category != 0) {
				category = await Category.FindAsync(_unitOfWork.GetRepository<Category>(), dto.Category);
				if (category == null) {
					return new GoodResponse(new CategoryNotExistsDto());
				}
			}

			// create new badge
			var pack = await BadgeUtil.AddMemoryBadgeAsync(_unitOfWork, dto, sender, receiver, category);
			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			// return single badge dto
			try {
				var data = await BadgeDtoUtil.GetMemoryBadgeDtoAsync(_unitOfWork, _mapper, pack);
				return new GoodResponse(new GoodDto("Memory badge placed", data));
			} catch (Exception ex) {
				return new GoodResponse(new OrdinaryDto(Errors.FailedToGetBadge, ex.Message));
			}
		}

		public async Task<ApiResponse> DeleteBadge(int id, DeleteBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var data = new DeleteBadgeSuccessDto {
				Errors = await BadgeUtil.DeleteBadgesAsync(_unitOfWork, dto.Badges, user, dto.Force)
			};

			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			var message = data.Errors.Count == 0 ? "Deletion succeeded" : "Deletion partial succeeded";

			return new GoodResponse(new GoodDto(message, data));
		}

		public async Task<ApiResponse> UpdateBadge(int id, UpdateBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
			if (badge == null) {
				return new GoodResponse(new BadgeNotExistsDto());
			}

			badge.Style = dto.Style;
			badge.IsPublic = dto.IsPublic;

			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			var data = new UpdateBadgeSuccessDto {
				IsPublic = badge.IsPublic,
				Style = badge.Style
			};
			return new GoodResponse(new GoodDto("Badge updated", data));
		}

		public async Task<ApiResponse> UpdateQuestionBadge(int id, UpdateQuestionBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
			if (badge == null) {
				return new GoodResponse(new BadgeNotExistsDto());
			}

			var payload = await QuestionPayload.FindAsync(
				_unitOfWork.GetRepository<QuestionPayload>(), badge.PayloadId);
			if (payload == null) {
				return new GoodResponse(new PayloadNotExistsDto());
			}

			payload.Answer = dto.Answer;
			badge.IsChecked = payload.Answer != null;

			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			var data = new UpdateQuestionBadgeSuccessDto {
				Answer = payload.Answer
			};
			return new GoodResponse(new GoodDto("Question Badge updated", data));
		}

		public async Task<ApiResponse> UpdateMemoryBadge(int id, UpdateMemoryBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
			if (badge == null) {
				return new GoodResponse(new BadgeNotExistsDto());
			}

			var payload = await MemoryPayload.FindAsync(
				_unitOfWork.GetRepository<MemoryPayload>(), badge.PayloadId);
			if (payload == null) {
				return new GoodResponse(new PayloadNotExistsDto());
			}

			payload.Memory = dto.Memory;

			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			var data = new UpdateMemoryBadgeSuccessDto {
				Memory = payload.Memory
			};
			return new GoodResponse(new GoodDto("Memory Badge updated", data));
		}

		public async Task<ApiResponse> MoveBadge(int id, MoveBadgeDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var badge = await Badge.FindAsync(_unitOfWork.GetRepository<Badge>(), dto.Id);
			if (badge == null) {
				return new GoodResponse(new BadgeNotExistsDto());
			}

			// Well, hope front end will handle requests that moves to itself.
			var name = "Default";
			if (dto.Category == 0) {
				badge.CategoryId = null;
			} else {
				var category = await Category.FindAsync(
					_unitOfWork.GetRepository<Category>(), dto.Category);
				if (category == null) {
					return new GoodResponse(new CategoryNotExistsDto());
				}
				badge.CategoryId = category.Id;
				name = category.Name;
			}

			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			return new GoodResponse(new GoodDto($"Badge moved to {name}"));
		}
	}
}
