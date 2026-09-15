using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.Repository.Interfaces;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {

        private readonly ILessonService lessonService;

        public LessonController(ILessonService lessonService) 
        {
            this.lessonService = lessonService;
        
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] LessonCreateDTO dto) 
        {
            var created = await lessonService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id = created.Id }, created);

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        { 
            var deleted = await lessonService.DeleteAsync(id);
            return NoContent();
        
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll() 
        {
            var lessons = await lessonService.GetAllAsync();
            return Ok(lessons);
        }

        [HttpGet("course/{CourseId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByCourse(int CourseId) 
        {
            var lessonCourse = await lessonService.GetByCourseAsync(CourseId);
            return Ok(lessonCourse);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id) 
        { 
            var lesson = await lessonService.GetByIdAsync(id);
            return Ok(lesson);
        
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] LessonUpdateDTO dto) 
        {
            var updated = await lessonService.UpdateAsync(id, dto);
            return Ok(updated);
        }
    }
}
