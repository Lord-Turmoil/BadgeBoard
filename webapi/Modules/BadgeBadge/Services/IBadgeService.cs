using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services
{
	/// <summary>
	/// For the creation of badges.
	/// </summary>
	public interface IBadgeService
	{
		// Here, all id is the sender's id. For requests that can
		// be anonymous (only add badge) 0 means anonymous.

		public Task<ApiResponse> AddQuestionBadge(int id, AddQuestionBadgeDto dto);
		public Task<ApiResponse> AddMemoryBadge(int id, AddMemoryBadgeDto dto);

		public Task<ApiResponse> DeleteBadge(int id, DeleteBadgeDto dto);
	}
}
