namespace CourseManagementSystem.Models.entities
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;

        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;

        public string CorrectAnswer { get; set; } = null!;
        public int Points { get; set; }

        //FK ->1:N
        public int TestId { get; set; }
        public Test Test { get; set; } = null!;

    }
}
