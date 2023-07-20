using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Extensions.Module;
using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Services.Utils;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

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

			var categories = await CategoryUtil.GetCategoryOfUserAsync(_unitOfWork.GetRepository<Category>(), user.Id);
			return new GoodResponse(new GoodWithDataDto(categories.Any(x => x.Name == name)));
		}

		public Task<ApiResponse> AddCategory(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> DeleteCategory(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> RenameCategory(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> UpdateCategory(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> MergeCategory(int id)
		{
			throw new NotImplementedException();
		}
	}
}
