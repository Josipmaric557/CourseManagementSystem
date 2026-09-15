namespace CourseManagementSystem.DTOs.Course
{
    public class CourseCreateDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool isPublihed { get; set; }
        public int CategoryId { get; set; }
    }
}
