namespace TaskManagement.Api.DTO.AuthDTO
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
