using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Lessons)
            .Include(c => c.Reviews)
            .Include(c => c.UserCourses)
            .FirstAsync(c => c.Id == course.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await GetByIdAsync(id);
            if (course is null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        => await _context.Courses.AnyAsync(c => c.Id == id);

        public async Task<IEnumerable<Course>> GetAllAsync()
        => await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Lessons)
            .Include(c => c.Reviews)
            .Include(c => c.UserCourses)
            .ToListAsync();

        public async Task<IEnumerable<Course>> GetByCategoryAsync(int CategoryId)
        => await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Lessons)
            .Include(c => c.Reviews)
            .Include(c => c.UserCourses)
            .Where(c => c.Category.Id == CategoryId)
            .ToListAsync();

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                .Include(c => c.Reviews)
                .Include(c => c.UserCourses)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                .Include(c => c.Reviews)
                .Include(c => c.UserCourses)
                .FirstAsync(c => c.Id == course.Id);
        }
    }
}
