using System.Net.Mail;
using System.Net;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IEmailOtpRepository _otpRepo;

        public EmailService(IEmailOtpRepository otpRepo, IConfiguration config)
        {
            _otpRepo = otpRepo;
            _config = config;
        }

        // Sinh OTP 6 chữ số
        private string GenerateOtp()
        {
            return Random.Shared.Next(100000, 999999).ToString();
        }

        // Gửi email OTP
        public async Task SendOtpEmailAsync(string toEmail)
        {
            // 1. Sinh OTP
            var otp = GenerateOtp();

            // 2. Lưu OTP vào DB
            var otpEntity = new EmailOtp
            {
                Email = toEmail,
                OtpCode = otp,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _otpRepo.CreateAsync(otpEntity);

            // 3. Lấy cấu hình email
            var emailSettings = _config.GetSection("EmailSettings");

            try
            {
                using var smtpClient = new SmtpClient
                {
                    Host = emailSettings["SmtpServer"],
                    Port = int.Parse(emailSettings["Port"]!),
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        emailSettings["Username"],
                        emailSettings["Password"]
                    )
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(
                        emailSettings["SenderEmail"]!,
                        emailSettings["SenderName"]
                    ),
                    Subject = "Mã xác thực OTP - Task Management System",
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                // 4. Body HTML đẹp
                message.Body = $@"
<html>
<head>
  <style>
    body {{
        font-family: Arial, sans-serif;
        background-color: #f4f4f4;
        margin: 0;
        padding: 0;
    }}
    .container {{
        max-width: 600px;
        margin: 30px auto;
        background-color: #ffffff;
        border-radius: 10px;
        box-shadow: 0 0 10px rgba(0,0,0,0.1);
        padding: 20px;
        text-align: center;
    }}
    h2 {{
        color: #333333;
    }}
    .otp-code {{
        display: inline-block;
        background-color: #007BFF;
        color: #ffffff;
        font-size: 32px;
        font-weight: bold;
        padding: 15px 30px;
        border-radius: 8px;
        margin: 20px 0;
        letter-spacing: 5px;
    }}
    p {{
        color: #555555;
        font-size: 16px;
    }}
    .footer {{
        font-size: 12px;
        color: #999999;
        margin-top: 20px;
    }}
  </style>
</head>
<body>
    <div class='container'>
        <h2>Task Management System</h2>
        <p>Xin chào,</p>
        <p>Mã OTP của bạn là:</p>
        <div class='otp-code'>{otp}</div>
        <p>OTP có hiệu lực trong 5 phút.<br/>Không chia sẻ mã này với bất kỳ ai.</p>
        <div class='footer'>
            Nếu bạn không yêu cầu OTP này, vui lòng bỏ qua email này.
        </div>
    </div>
</body>
</html>
";

                // 5. Gửi mail
                await smtpClient.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException("Gửi OTP thất bại. Vui lòng thử lại sau.", ex);
            }
        }

        // Verify OTP
        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var otpEntity = await _otpRepo.GetValidOtpAsync(email, otp);
            if (otpEntity == null)
                return false;

            otpEntity.IsUsed = true;
            await _otpRepo.UpdateAsync(otpEntity);

            return true;
        }
    }
}
