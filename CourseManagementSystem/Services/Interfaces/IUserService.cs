using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Auth;
using CourseManagementSystem.DTOs.UserD;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO> UpdateAsync(int id, UserUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
