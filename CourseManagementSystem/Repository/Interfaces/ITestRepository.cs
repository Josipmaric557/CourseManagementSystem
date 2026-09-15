using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface ITestRepository
    {
        Task<Test?> GetByIdAsync(int id);
        Task<IEnumerable<Test>> GetAllAsync();
        Task<IEnumerable<Test>> GetByLessonAsync(int lessonId);
        Task<Test> CreateAsync(Test test);
        Task<Test> UpdateAsync(Test test);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
