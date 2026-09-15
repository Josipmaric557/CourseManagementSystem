using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Test;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface ITestService
    {
        Task<TestResponseDTO> CreateAsync(TestCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TestResponseDTO>> GetAllAsync();
        Task<TestResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TestResponseDTO>> GetByLessonAsync(int lessonId);
        Task<TestResponseDTO> UpdateAsync(int id, TestUpdateDTO dto);
    }
}
