using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);

            if (user is null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        => await _context.Users
            .AnyAsync(u => u.Email == email);

        public async Task<bool> ExistsByUsernameAsync(string username)
        => await _context.Users
            .AnyAsync(u => u.Username == username);

        public async Task<IEnumerable<User>> GetAllAsync()
        => await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();

        public async Task<User?> GetByEmailAsync(string email)
        =>await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(int id)
         => await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        => await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);


        public async Task<User?> GetByUsernameAsync(string username)
        => await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
