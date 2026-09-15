namespace CourseManagementSystem.DTOs.Review
{
    public class ReviewUpdateDTO
    {
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
