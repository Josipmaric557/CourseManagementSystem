namespace CourseManagementSystem.DTOs.TestQuestion
{
    public class TestQuestionUpdateDTO
    {
        public string? Question { get; set; }

        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }

        public string? CorrectAnswer { get; set; }
        public int? Points { get; set; }
        public int? TestId { get; set; }
    }
}
