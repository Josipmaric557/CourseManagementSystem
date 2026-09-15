using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Auth;
using CourseManagementSystem.DTOs.Test;
using CourseManagementSystem.DTOs.UserD;
using CourseManagementSystem.Services.Implementation;
using CourseManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }
    
    
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<UserResponseDTO>> GetById(int id)
    {
        var users_ = await userService.GetByIdAsync(id);
        return Ok(users_);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll()
    {
        var users_ = await userService.GetAllAsync();
        return Ok(users_);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseDTO>> updateAsync(int id, [FromBody] UserUpdateDTO dto)
    {
        var updated = await userService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> deleteAsync(int id)
    {
        var deleted = await userService.DeleteAsync(id);

        return NoContent();
    }
}