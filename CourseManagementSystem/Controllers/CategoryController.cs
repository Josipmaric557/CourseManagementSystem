using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Category;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id) 
        {
            var category = await categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO dto) 
        { 
            var created = await categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id}, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDTO dto) 
        {
            var updated = await categoryService.UpdateAsync(id, dto);
            return Ok(updated);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
            var deleted = await categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
