using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task<IEnumerable<Lesson>> GetByCourseAsync(int Courseid);
        Task<Lesson> CreateAsync(Lesson lesson);
        Task<Lesson> UpdateAsync(Lesson lesson);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
