using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;

namespace CourseManagementSystem.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(RoleE name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<bool> ExistsByIdAsync(int id);

    }
}
