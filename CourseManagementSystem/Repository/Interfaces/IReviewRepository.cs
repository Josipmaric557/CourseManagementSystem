using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetByUsers(int UserId);
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetByCourses(int CourseId);
        Task<Review?> GetByUserAndCourseAsync(int UserId, int CourseId);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByUserAndCourseAsync(int UserId, int CourseId);

    }
}
