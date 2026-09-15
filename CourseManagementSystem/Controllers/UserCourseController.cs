using System.Threading.Tasks;
using CourseManagementSystem.DTOs.UserCourse;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCourseController : ControllerBase
    {

        private readonly IUserCourseService userCourseService;

        public UserCourseController(IUserCourseService userCourseService)
        {
            this.userCourseService = userCourseService;
        }

        [HttpPost("{userId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int userId, [FromBody] UserCourseCreateDTO dto)
        {
            var created = await userCourseService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetByUser), new { userId = created.UserId }, created);
        }
        
        [HttpDelete("user/{userId:int}/course/{courseId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int userId, int courseId)
        {
            var deleted = await userCourseService.DeleteAsync(userId, courseId);
            return NoContent();
        }

        [HttpGet("course/{courseId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var course = await userCourseService.GetByCourseAsync(courseId);
            return Ok(course);
        }

        [HttpGet("user/{userId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var user = await userCourseService.GetByUserAsync(userId);
            return Ok(user);

        }

        [HttpPut("user/{userId:int}/course/{courseId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int userId, int courseId, UserCourseUpdateDTO dto) 
        { 
            var updated = await userCourseService.UpdateAsync(userId, courseId, dto);
            return Ok(updated);
        
        }
    }
}
