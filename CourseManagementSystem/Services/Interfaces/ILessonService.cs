using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface ILessonService
    {
        Task<LessonResponseDTO> CreateAsync(LessonCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<LessonResponseDTO>> GetAllAsync();
        Task<IEnumerable<LessonResponseDTO>> GetByCourseAsync(int Courseid);
        Task<LessonResponseDTO?> GetByIdAsync(int id);
        Task<LessonResponseDTO> UpdateAsync(int id, LessonUpdateDTO dto);
    }
}
