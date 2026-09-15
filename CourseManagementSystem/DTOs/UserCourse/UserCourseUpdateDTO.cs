using CourseManagementSystem.Enums;

namespace CourseManagementSystem.DTOs.UserCourse
{
    public class UserCourseUpdateDTO
    {
        public int ProgressPercent { get; set; }
        public string? Status { get; set; }
    }
}
