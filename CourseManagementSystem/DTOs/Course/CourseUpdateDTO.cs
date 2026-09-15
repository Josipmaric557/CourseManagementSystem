namespace CourseManagementSystem.DTOs.Course
{
    public class CourseUpdateDTO
    {
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public bool? IsPublished { get; set; }
        public int? CategoryId { get; set; }
    }
}
