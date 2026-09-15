using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Test;
using CourseManagementSystem.DTOs.TestQuestion;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionController : ControllerBase
    {

        private readonly ITestQuestionService testQuestionService;

        public TestQuestionController(ITestQuestionService testQuestionService) 
        {
            this.testQuestionService = testQuestionService;
        
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TestQuestionCreateDTO dto) 
        {
            var created = await testQuestionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
            var deleted = await testQuestionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll()
        {
            var tqAll = await testQuestionService.GetAllAsync();
            return Ok(tqAll);
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id) 
        {
            var testQuestion = await testQuestionService.GetByIdAsync(id);
            return Ok(testQuestion);
        }


        [HttpGet("test/{testId:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByTest(int testId) 
        {
            var test = await testQuestionService.GetByTestAsync(testId);
            return Ok(test);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] TestQuestionUpdateDTO dto) 
        {
            var updated = await testQuestionService.UpdateAsync(id, dto);
            return Ok(updated);
        }



    }
}
