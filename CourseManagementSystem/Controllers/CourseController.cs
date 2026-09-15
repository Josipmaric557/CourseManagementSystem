using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Course;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService) 
        {
            this.courseService = courseService;
        
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CourseCreateDTO dto) 
        { 
            var created = await courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id = created.Id }, created);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id) 
        {
            var deleted = await courseService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll() 
        {
            var courses = await courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("category/{categoryId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByCategory(int categoryId) 
        {
            var courseCategory = await courseService.GetByCategoryAsync(categoryId);
            return Ok(courseCategory);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id) 
        {
            var course = await courseService.GetByIdAsync(id);
            return Ok(course);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDTO dto) 
        { 
            var updated = await courseService.UpdateAsync(id, dto);
            return Ok(updated);
        }
    }
}
