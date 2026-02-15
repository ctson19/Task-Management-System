using TaskManagement.Api.DTO.Notifications;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationResponseDto>> GetMyNotificationsAsync(Guid userId);
        Task CreateAsync(Guid userId, string title, string? content = null);
        Task MarkAsReadAsync(Guid notificationId, Guid userId);
        Task MarkAllAsReadAsync(Guid userId);
    }
}


