namespace TaskManagement.Api.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string toEmail);
        Task<bool> VerifyOtpAsync(string email, string otp);
    }
}
