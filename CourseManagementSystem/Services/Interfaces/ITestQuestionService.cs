using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.TestQuestion;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface ITestQuestionService
    {
        Task<TestQuestionResponseDTO> CreateAsync(TestQuestionCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<TestQuestionResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TestQuestionResponseDTO>> GetAllAsync();
        Task<IEnumerable<TestQuestionResponseDTO>> GetByTestAsync(int testId);
        Task<TestQuestionResponseDTO> UpdateAsync(int id, TestQuestionUpdateDTO dto);
    }
}
