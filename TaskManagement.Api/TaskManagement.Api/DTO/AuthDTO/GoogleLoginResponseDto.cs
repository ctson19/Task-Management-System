namespace TaskManagement.Api.DTO.AuthDTO
{
    public class GoogleLoginResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiredAt { get; set; }
    }
}
