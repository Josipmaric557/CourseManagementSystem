using System.Threading.Tasks;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll() 
        {
            var roles = await roleService.GetAllAsync();
            return Ok(roles);
        
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(int id) 
        { 
            var role = await roleService.GetByIdAsync(id);
            return Ok(role);
        
        }

        [HttpGet("/roleE")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByName(RoleE roleE) 
        {
            var roleName = await roleService.GetByNameAsync(roleE);
            return Ok(roleName);
        }
    }
}
