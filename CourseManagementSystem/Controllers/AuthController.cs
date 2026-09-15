using CourseManagementSystem.DTOs.Auth;
using CourseManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await authService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            var result = await authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Logout()
        {
            
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await authService.LogoutAsync(userId);
            return NoContent();
        }
    }
}
