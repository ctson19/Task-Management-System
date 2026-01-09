using TaskManagement.Api.DTO.AuthDTO;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    }
}
