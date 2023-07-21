using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BadgeBoard.Api.Modules.BadgeBadge.Dtos.Badge;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeGlobal.Exceptions;
using BadgeBoard.Api.Modules.BadgeUser.Dtos;
using BadgeBoard.Api.Modules.BadgeUser.Models;

namespace BadgeBoard.Api.Modules.BadgeBadge.Services.Utils
{
	public static class BadgeUtil
	{
		public static async Task<QuestionBadgePack> AddQuestionBadgeAsync(
			IUnitOfWork unitOfWork, AddQuestionBadgeDto dto,
			User? sender, User receiver, Category? category)
		{
			var payload = await QuestionPayload.CreateAsync(unitOfWork.GetRepository<QuestionPayload>(), dto.Question);
			var badge = await Badge.CreateAsync(unitOfWork.GetRepository<Badge>(),
				dto.Type, payload.Id, sender, receiver, category, dto.Style);

			return new QuestionBadgePack { Badge = badge, Payload = payload };
		}

		public static async Task<MemoryBadgePack> AddMemoryBadgeAsync(
			IUnitOfWork unitOfWork, AddMemoryBadgeDto dto,
			User? sender, User receiver, Category? category)
		{
			var payload = await MemoryPayload.CreateAsync(unitOfWork.GetRepository<MemoryPayload>(), dto.Memory);
			var badge = await Badge.CreateAsync(unitOfWork.GetRepository<Badge>(),
				dto.Type, payload.Id, sender, receiver, category, dto.Style);

			return new MemoryBadgePack { Badge = badge, Payload = payload };
		}

		public static void DeleteBadge(IUnitOfWork unitOfWork, Badge badge)
		{
			unitOfWork.GetRepository<Badge>().Delete(badge.Id);
			if (badge.Type == Badge.Types.Question) {
				unitOfWork.GetRepository<QuestionPayload>().Delete(badge.PayloadId);
			} else {
				unitOfWork.GetRepository<MemoryPayload>().Delete(badge.PayloadId);
			}
		}

		public static async Task<List<DeleteBadgeErrorData>> DeleteBadgesAsync(IUnitOfWork unitOfWork, IEnumerable<int> badges, User user, bool force = false)
		{
			var errors = new List<DeleteBadgeErrorData>();
			var repo = unitOfWork.GetRepository<Badge>();

			foreach (var badgeId in badges) {
				var badge = await Badge.FindAsync(repo, badgeId);
				if (badge == null) {
					errors.Add(new DeleteBadgeErrorData {
						Id = badgeId,
						Message = "Badge does not exist"
					});
					continue;
				}

				if (badge.UserId == user.Id) {
					// delete badge of user him/her self
					DeleteBadge(unitOfWork, badge);
				} else if (user.IsAdmin) {
					// admin can delete other user's badge in force mode
					if (force) {
						DeleteBadge(unitOfWork, badge);
					} else {
						errors.Add(new DeleteBadgeErrorData {
							Id = badgeId,
							Message = "Not in force mode"
						});
					}
				} else {
					// no permission
					errors.Add(new DeleteBadgeErrorData {
						Id = badgeId,
						Message = "Permission denied"
					});
				}
			}

			return errors;
		}
	}
}
