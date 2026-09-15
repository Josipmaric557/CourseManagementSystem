using System.Collections.Generic;

namespace CourseManagementSystem.Models.entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //1:N
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
