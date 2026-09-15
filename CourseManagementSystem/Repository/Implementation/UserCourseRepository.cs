using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class UserCourseRepository : IUserCourseRepository
    {
        private AppDbContext _context;
        public UserCourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserCourse> CreateAsync(UserCourse userCourse)
        { 
            _context.UserCourses.Add(userCourse);
            await _context.SaveChangesAsync();
            return await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .FirstAsync(uc =>
                    uc.UserId == userCourse.UserId &&
                    uc.CourseId == userCourse.CourseId);
        }

        public async Task<bool> DeleteAsync(int userId, int courseId)
        {
            var userCourse = await GetByUserAndCourseAsync(userId, courseId);

            if (userCourse is null) return false;

            _context.UserCourses.Remove(userCourse);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(int userId, int courseId)
        => _context.UserCourses.AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

        public async Task<IEnumerable<UserCourse>> GetByCourseAsync(int courseId)
        => await _context.UserCourses
            .Include(uc => uc.User)
            .Include(uc => uc.Course)
            .Where(uc => uc.CourseId == courseId)
            .ToListAsync();

        public async Task<UserCourse?> GetByUserAndCourseAsync(int userId, int courseId)
        => await _context.UserCourses
            .Include(uc => uc.User)
            .Include(uc => uc.Course)
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

        public async Task<IEnumerable<UserCourse>> GetByUserAsync(int userId)
        => await _context.UserCourses
            .Include(uc => uc.Course)
                .ThenInclude(c => c.Category)
            .Include(uc => uc.User)
            .Where(uc => uc.UserId == userId)
            .ToListAsync();

        public async Task<UserCourse> UpdateAsync(UserCourse userCourse)
        {
            _context.UserCourses.Update(userCourse);
            await _context.SaveChangesAsync();
            return userCourse;
        }
    }
}
