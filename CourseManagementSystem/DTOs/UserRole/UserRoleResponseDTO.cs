using System;

namespace CourseManagementSystem.DTOs.UserRole
{
    public class UserRoleResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;

        public int RoleId { get; set; }
        public string RoleTitle { get; set; } = null!;

        public DateTime AssignedAt { get; set; }
    }
}
