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

			var message = data.Errors.Count == 0 ? "Deletion succeeded" : "Deletion partial succeeded";

			return new GoodResponse(new GoodDto(message, data));
		}

		public Task<ApiResponse> UpdateQuestionBadge(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> UpdateMemoryBadge(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> MoveBadge(int id)
		{
			throw new NotImplementedException();
		}
	}
}
