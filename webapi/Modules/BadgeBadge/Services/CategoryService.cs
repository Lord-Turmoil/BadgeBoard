using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;
using BadgeBoard.Api.Modules.BadgeGlobal.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.JSInterop.Infrastructure;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services
{
	public class CategoryService : BaseService, ICategoryService
	{
		protected CategoryService(IServiceProvider provider, IUnitOfWork unitOfWork, IMapper mapper) : base(provider, unitOfWork, mapper)
		{
		}

		public async Task<ApiResponse> Exists(int id, string name)
		{
			var repo = _unitOfWork.GetRepository<User>();
			var user = await User.FindAsync(repo, id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var exists = await CategoryUtil.HasCategoryOfUserAsync(_unitOfWork.GetRepository<Category>(), user.Id, name);
			return new GoodResponse(new GoodWithDataDto(exists));
		}

		public async Task<ApiResponse> AddCategory(int id, AddCategoryDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user == null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var repo = _unitOfWork.GetRepository<Category>();
			if (await CategoryUtil.HasCategoryOfUserAsync(repo, id, dto.Name)) {
				return new GoodResponse(new CategoryAlreadyExistsDto());
			}

			var category = await Category.CreateAsync(repo, dto.Name, user);
			CategoryUtil.UpdateCategory(category, dto);
			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			var data = _mapper.Map<Category, CategoryDto>(category);
			return new GoodResponse(new GoodDto("New category options", data));
		}

		public Task<ApiResponse> DeleteCategory(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDto dto)
		{
			if (!dto.Format().Verify()) {
				return new BadRequestResponse(new BadRequestDto());
			}

			var user = await User.FindAsync(_unitOfWork.GetRepository<User>(), id);
			if (user != null) {
				return new GoodResponse(new UserNotExistsDto());
			}

			var repo = _unitOfWork.GetRepository<Category>();
			var category = await Category.FindAsync(repo, dto.Id, true);
			if (category == null) {
				return new GoodResponse(new CategoryNotExistsDto());
			}

			// Check name duplication
			if (category.Name != dto.Name) {
				if (await CategoryUtil.HasCategoryOfUserAsync(repo, id, dto.Name)) {
					return new GoodResponse(new CategoryAlreadyExistsDto());
				}
			}

			// Update whole category.
			CategoryUtil.UpdateCategory(category, dto);
			try {
				await _unitOfWork.SaveChangesAsync();
			} catch (Exception ex) {
				return new InternalServerErrorResponse(new FailedToSaveChangesDto(data: ex));
			}

			var data = _mapper.Map<Category, CategoryDto>(category);
			return new GoodResponse(new GoodDto("Category updated", data));
		}

		public Task<ApiResponse> MergeCategory(int id)
		{
			throw new NotImplementedException();
		}
	}
}
