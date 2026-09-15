using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Role;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface IRoleService
    {
        
        Task<IEnumerable<RoleResponseDTO>> GetAllAsync();
        Task<RoleResponseDTO?> GetByIdAsync(int id);
        Task<RoleResponseDTO?> GetByNameAsync(RoleE name);
    }
}
