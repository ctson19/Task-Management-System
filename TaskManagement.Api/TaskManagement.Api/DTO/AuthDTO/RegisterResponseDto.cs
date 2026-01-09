namespace TaskManagement.Api.DTO.AuthDTO
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
