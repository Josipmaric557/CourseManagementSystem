using System;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.DTOs.Review
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
    }
}
