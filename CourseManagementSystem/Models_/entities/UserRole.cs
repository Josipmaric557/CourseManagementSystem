using System;
using CourseManagementSystem.DbContextModel;

namespace CourseManagementSystem.Models.entities
{
    public class UserRole : AuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
