namespace CourseManagementSystem.DTOs.Lesson
{
    public class LessonUpdateDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? OrderIndex { get; set; }
        public int? DurationMinutes { get; set; }
        public int? CourseId { get; set; }
    }
}
