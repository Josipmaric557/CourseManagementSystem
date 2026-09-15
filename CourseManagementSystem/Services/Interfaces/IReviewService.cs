using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Review;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO> CreateAsync(int userId, ReviewCreateDTO dto);
        Task<bool> DeleteAsync(int id, int userId);
        Task<IEnumerable<ReviewResponseDTO>> GetAllAsync();
        Task<IEnumerable<ReviewResponseDTO>> GetByCourses(int courseId);
        Task<ReviewResponseDTO?> GetByIdAsync(int id);
        Task<ReviewResponseDTO?> GetByUserAndCourseAsync(int userId, int courseId);
        Task<IEnumerable<ReviewResponseDTO>> GetByUsers(int userId);
        Task<ReviewResponseDTO> UpdateAsync(int id, int userId, ReviewUpdateDTO dto);
    }
}