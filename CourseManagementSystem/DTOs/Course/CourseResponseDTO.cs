using System;

namespace CourseManagementSystem.DTOs.Course
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int UserRoleCounts { get; set; }
        public int LessonsCount { get; set; }
        public int ReviewsCount { get; set; }
    }
}
