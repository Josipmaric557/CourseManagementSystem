using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.UserCourse;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface IUserCourseService
    {
        Task<UserCourseResponseDTO> CreateAsync(int userId, UserCourseCreateDTO dto);
        Task<bool> DeleteAsync(int userId, int courseId);
        Task<IEnumerable<UserCourseResponseDTO>> GetByCourseAsync(int courseId);
        Task<UserCourseResponseDTO?> GetByUserAndCourseAsync(int userId, int courseId);
        Task<IEnumerable<UserCourseResponseDTO>> GetByUserAsync(int userId);
        Task<UserCourseResponseDTO> UpdateAsync(int userId, int courseId,UserCourseUpdateDTO dto);
    }
}
