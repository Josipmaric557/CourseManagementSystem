using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CourseManagementSystem.DTOs.Lesson
{
    public class LessonCreateDTO
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int OrderIndex { get; set; }
        public int DurationMinutes { get; set; }
        public int CourseId { get; set; }
    }
}
