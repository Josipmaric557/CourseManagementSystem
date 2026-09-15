using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Test;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;

        public TestController(ITestService testService) 
        { 
            this.testService = testService;
        
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TestCreateDTO dto) 
        {
            var created = await testService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
            var deleted = await testService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll() 
        {
            var tests = await testService.GetAllAsync();
            return Ok(tests);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id) 
        { 
            var test = await testService.GetByIdAsync(id);
            return Ok(test);
        }

        [HttpGet("lesson/{lessonId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByLesson(int lessonId) 
        {
            var testLesson = await testService.GetByLessonAsync(lessonId);
            return Ok(testLesson);
        
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] TestUpdateDTO dto) 
        {
            var updated = await testService.UpdateAsync(id, dto);
            return Ok(updated);
        
        }


    }
}
