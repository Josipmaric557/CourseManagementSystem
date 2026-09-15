namespace CourseManagementSystem.DTOs.TestQuestion
{
    public class TestQuestionResponseDTO
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;

        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }

        public string CorrectAnswer { get; set; } = null!;
        public int Points { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; } = null!;
    }
}
