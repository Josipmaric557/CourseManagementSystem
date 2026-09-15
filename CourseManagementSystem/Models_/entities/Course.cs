using System;
using System.Collections.Generic;
using CourseManagementSystem.DbContextModel;

namespace CourseManagementSystem.Models.entities
{
    public class Course : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //FK -> 1:N
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //M:N
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        //1:N
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        //1:N
        public ICollection<Review> Reviews { get; set; } = new List<Review>();



    }
}
