using CourseManagementSystem.DTOs.Review;
using CourseManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int userId, [FromBody] ReviewCreateDTO dto)
        {
           
            var created = await reviewService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, int userId)
        {
            
            await reviewService.DeleteAsync(id, userId);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await reviewService.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("course/{courseId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByCourses(int courseId)
        {
            var reviews = await reviewService.GetByCourses(courseId);
            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await reviewService.GetByIdAsync(id);
            return Ok(review);
        }

        [HttpGet("user/{userId:int}/course/{courseId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByUserAndCourse(int userId, int courseId)
        {
            var review = await reviewService.GetByUserAndCourseAsync(userId, courseId);

            if (review is null)
                return NotFound();

            return Ok(review);
        }

        [HttpGet("user/{userId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByUsers(int userId)
        {
            var reviews = await reviewService.GetByUsers(userId);
            return Ok(reviews);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, int UserId, [FromBody] ReviewUpdateDTO dto)
        {
        
            var updated = await reviewService.UpdateAsync(id, UserId, dto);
            return Ok(updated);
        }

        
    }
}