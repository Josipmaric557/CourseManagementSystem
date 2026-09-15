using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.TestQuestion;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface ITestQuestionRepository
    {
        Task<TestQuestion?> GetByIdAsync(int id);
        Task<IEnumerable<TestQuestion>> GetByTestAsync(int testId);
        Task<IEnumerable<TestQuestion>> GetAllAsync();
        Task<TestQuestion> CreateAsync(TestQuestion testQuestion);
        Task<TestQuestion> UpdateAsync(TestQuestion testQuestion);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
