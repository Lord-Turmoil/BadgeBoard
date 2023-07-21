using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services
{
	/// <summary>
	/// For the creation of badges. All id is the sender's id. For requests
	/// that can be anonymous (only add badge), 0 means anonymous.
	/// </summary>
	public interface IBadgeService
	{
		public Task<ApiResponse> AddQuestionBadge(int id, AddQuestionBadgeDto dto);
		public Task<ApiResponse> AddMemoryBadge(int id, AddMemoryBadgeDto dto);

		public Task<ApiResponse> DeleteBadge(int id, DeleteBadgeDto dto);

		public Task<ApiResponse> UpdateQuestionBadge(int id);
		public Task<ApiResponse> UpdateMemoryBadge(int id);

		// Move badge to another category.
		public Task<ApiResponse> MoveBadge(int id);
	}
}
