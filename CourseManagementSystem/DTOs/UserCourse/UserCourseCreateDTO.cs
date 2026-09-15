using System;
using CourseManagementSystem.Enums;

namespace CourseManagementSystem.DTOs.UserCourse
{
    public class UserCourseCreateDTO
    {
        
        public int CourseId { get; set; }
        public DateTime StartedAt { get; set; }
        public StartedStatus Status { get; set; }
        public int ProgressPercent { get; set; } 
    }
}
