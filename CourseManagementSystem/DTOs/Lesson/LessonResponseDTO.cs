namespace CourseManagementSystem.DTOs.Lesson
{
    public class LessonResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int OrderIndex { get; set; }
        public int DurationMinutes { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public int TestsCount { get; set; }
    }
}
