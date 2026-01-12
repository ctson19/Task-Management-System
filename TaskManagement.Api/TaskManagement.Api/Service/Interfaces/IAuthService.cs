using System.Security.Claims;
using TaskManagement.Api.DTO.AuthDTO;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        string GenerateJwtToken(User user);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<GoogleLoginResponseDto> GoogleLoginAsync(ClaimsPrincipal principal);
    }
}
