using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Api.DTO.AuthDTO;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");

            if (!user.IsActive)
                throw new InvalidOperationException("Tài khoản đã bị khóa");

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                AccessToken = token,
                ExpiredAt = DateTime.UtcNow.AddHours(2)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("username", user.UserName)
        };

            // 🎯 ADD ROLE CLAIMS
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // 1️⃣ Check OTP đã verify chưa
            var isVerified = await _userRepository.IsEmailVerifiedAsync(request.Email);
            if (!isVerified)
                throw new Exception("Email chưa được xác thực OTP");

            // 2️⃣ Check user tồn tại
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Email already exists");

            // 3️⃣ Tạo user
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);

            return new RegisterResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }


    }
}
