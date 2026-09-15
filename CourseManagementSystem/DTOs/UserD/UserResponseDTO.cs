using System;

namespace CourseManagementSystem.DTOs.UserD
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public int UserRolesCount { get; set; }
        public int UserCoursesCount { get; set; }
        public int ReviewesCount { get; set; }

    }
}
