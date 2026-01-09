using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, string confirmPassword)
        {
            // 1. Lấy user
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User không tồn tại");

            // 2. Kiểm tra password hiện tại
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                throw new InvalidOperationException("Mật khẩu hiện tại không đúng");

            // 3. Kiểm tra confirm password
            if (newPassword != confirmPassword)
                throw new InvalidOperationException("Mật khẩu mới và xác nhận không khớp");

            // 4. Hash password mới
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // 5. Update DB
            await _userRepo.UpdateAsync(user);

            return true;
        }
    }

}
