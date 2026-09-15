namespace CourseManagementSystem.DTOs.Test
{
    public class TestResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int PassingScore { get; set; }
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = null!;
        public int QuestionsCount { get; set; }
    }
}
