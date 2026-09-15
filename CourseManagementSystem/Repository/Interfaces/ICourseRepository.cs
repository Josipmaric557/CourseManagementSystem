using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<IEnumerable<Course>> GetByCategoryAsync(int CategoryId);
        
        Task<Course> CreateAsync(Course course);
        Task<Course> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
