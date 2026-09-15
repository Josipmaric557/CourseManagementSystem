using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _contex;
        public RoleRepository(AppDbContext contex)
        {
            _contex = contex;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        => await _contex.Roles.AnyAsync(r => r.Id == id);

        public async Task<IEnumerable<Role>> GetAllAsync()
        => await _contex.Roles
            .Include(r => r.UserRoles)
            .ToListAsync();

        public async Task<Role?> GetByIdAsync(int id)
        => await _contex.Roles.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Role?> GetByNameAsync(RoleE name)
        {
            return await _contex.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
