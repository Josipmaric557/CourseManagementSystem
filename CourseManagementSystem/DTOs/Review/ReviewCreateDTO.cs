namespace CourseManagementSystem.DTOs.Review
{
    public class ReviewCreateDTO
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; } 
    }
}
