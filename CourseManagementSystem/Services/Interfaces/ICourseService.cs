using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Course;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface ICourseService
    {
        Task<CourseResponseDTO> CreateAsync(CourseCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CourseResponseDTO>> GetAllAsync();
        Task<IEnumerable<CourseResponseDTO>> GetByCategoryAsync(int CategoryId);
        Task<CourseResponseDTO?> GetByIdAsync(int id);
        Task<CourseResponseDTO> UpdateAsync(int id, CourseUpdateDTO dto);
    }
}
