using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Category;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Services.Implementation
{
    public interface ICategoryService
    {
        Task<CategoryResponseDTO> CreateAsync(CategoryCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CategoryResponseDTO>> GetAllAsync();
        Task<CategoryResponseDTO?> GetByIdAsync(int id);
        Task<CategoryResponseDTO> UpdateAsync(int id, CategoryUpdateDTO dto);

    }
}
