namespace CourseManagementSystem.DTOs.TestQuestion
{
    public class TestQuestionCreateDTO
    {
        public string Question { get; set; } = null!;

        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;

        public string CorrectAnswer { get; set; } = null!;
        public int Points { get; set; }
        public int TestId { get; set; }
    }
}
