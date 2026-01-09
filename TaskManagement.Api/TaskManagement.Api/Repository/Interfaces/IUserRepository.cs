using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);

        Task<bool> IsEmailVerifiedAsync(string email);

        Task<User?> GetByIdAsync(Guid userId);
        Task UpdateAsync(User user);
    }
}
