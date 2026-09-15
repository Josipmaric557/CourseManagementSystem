using System.Collections.Generic;

namespace CourseManagementSystem.Models.entities
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PassingScore { get; set; }

        //FK -> 1:N
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        //1:N
        public ICollection<TestQuestion> Questions { get; set; } = new List<TestQuestion>();
    }
}
