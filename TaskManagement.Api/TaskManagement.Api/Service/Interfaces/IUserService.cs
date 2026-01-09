namespace TaskManagement.Api.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, string confirmPassword);
    }
}
