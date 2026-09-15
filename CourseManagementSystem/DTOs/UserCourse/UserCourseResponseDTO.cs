using System;
using CourseManagementSystem.Enums;

namespace CourseManagementSystem.DTOs.UserCourse
{
    public class UserCourseResponseDTO
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        public StartedStatus Status { get; set; } = StartedStatus.Active;
        public int ProgressPercent { get; set; } = 0;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int CourseId { get; set; } 
        public string CourseTitle { get; set; } = null!;
    }
}
