using CourseManagementSystem.Enums;

namespace CourseManagementSystem.DTOs.Auth
{
    public class RegisterDTO
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public RoleE role { get; set; }
    }
}
