using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.DTO.AuthDTO;
using TaskManagement.Api.Models;
using TaskManagement.Api.Service.Implementations;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService,IUserService userService)
        {
            _authService = authService;
            _emailService = emailService;   
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra" });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // JWT stateless → client tự xóa token
            return Ok("Logout thành công");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }


        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDTO request)
        {
            try
            {
                await _emailService.SendOtpEmailAsync(request.Email);
                return Ok(new { message = "OTP đã gửi thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Gửi OTP thất bại. Vui lòng thử lại sau." });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDTO request)
        {
            var result = await _emailService.VerifyOtpAsync(request.Email, request.OtpCode);
            if (!result)
                return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn" });

            return Ok(new { message = "Email đã xác thực thành công" });
        }


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO request)
        {
            // Lấy userId từ JWT claim (NameIdentifier)
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "User không hợp lệ" });

            try
            {
                await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword, request.ConfirmPassword);
                return Ok(new { message = "Đổi mật khẩu thành công" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra. Vui lòng thử lại sau." });
            }
        }


        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-callback"
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("External");

            if (!result.Succeeded)
                return Unauthorized();

            var response = await _authService.GoogleLoginAsync(result.Principal);

            return Ok(response);
        }


    }
}
