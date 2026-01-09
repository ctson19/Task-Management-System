using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface IEmailOtpRepository
    {
        Task CreateAsync(EmailOtp otp);
        Task<EmailOtp?> GetValidOtpAsync(string email, string otpCode);
        Task UpdateAsync(EmailOtp otp);
    }
}
