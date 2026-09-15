using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Auth;

namespace CourseManagementSystem.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(int userId);
    }
}
