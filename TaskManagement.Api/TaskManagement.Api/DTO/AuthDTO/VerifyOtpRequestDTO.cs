namespace TaskManagement.Api.DTO.AuthDTO
{
    public class VerifyOtpRequestDTO
    {
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
    }
}
