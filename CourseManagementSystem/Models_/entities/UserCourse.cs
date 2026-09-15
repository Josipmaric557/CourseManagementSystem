using System;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Enums;

namespace CourseManagementSystem.Models.entities
{
    public class UserCourse : AuditableEntity
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public StartedStatus Status { get; set; } = StartedStatus.Active;

        public int ProgressPercent { get; set; } = 0;

        //FK->User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        //FK->Course
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

    }
}
