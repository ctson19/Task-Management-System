using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByUserAsync(Guid userId);
        Task AddAsync(Notification notification);
        Task MarkAsReadAsync(Guid notificationId, Guid userId);
        Task MarkAllAsReadAsync(Guid userId);
    }
}


