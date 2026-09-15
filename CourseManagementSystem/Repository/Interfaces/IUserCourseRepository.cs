using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.Models.entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface IUserCourseRepository
    {
        Task<UserCourse?> GetByUserAndCourseAsync(int userId, int courseId);
        Task<IEnumerable<UserCourse>> GetByUserAsync(int userId);
        Task<IEnumerable<UserCourse>> GetByCourseAsync(int courseId);
        Task<UserCourse> CreateAsync(UserCourse userCourse);
        Task<UserCourse> UpdateAsync(UserCourse userCourse);
        Task<bool> DeleteAsync(int userId, int courseId);
        Task<bool> ExistsAsync(int userId, int courseId);
    }
}
