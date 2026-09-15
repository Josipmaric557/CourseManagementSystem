using System.Collections.Generic;

namespace CourseManagementSystem.Models.entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int OrderIndex { get; set; }
        public int DurationMinutes { get; set; }

        //FK ->1:N
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        //1:N
        public ICollection<Test> Tests { get; set; } = new List<Test>();

    }
}