using TaskManagement.Api.DTO.Notifications;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationService(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        public async Task<List<NotificationResponseDto>> GetMyNotificationsAsync(Guid userId)
        {
            var list = await _notificationRepo.GetByUserAsync(userId);
            return list.Select(MapToDto).ToList();
        }

        public async Task CreateAsync(Guid userId, string title, string? content = null)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepo.AddAsync(notification);
        }

        public Task MarkAsReadAsync(Guid notificationId, Guid userId)
        {
            return _notificationRepo.MarkAsReadAsync(notificationId, userId);
        }

        public Task MarkAllAsReadAsync(Guid userId)
        {
            return _notificationRepo.MarkAllAsReadAsync(userId);
        }

        private static NotificationResponseDto MapToDto(Notification n)
        {
            return new NotificationResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            };
        }
    }
}


