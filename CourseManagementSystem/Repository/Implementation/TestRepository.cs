using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;
        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Test> CreateAsync(Test test)
        { 
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
            return await _context.Tests
                .Include(t => t.Lesson)
                .Include(t => t.Questions)
                .FirstAsync(t => t.Id == test.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var test = await GetByIdAsync(id);

            if (test is null) return false;

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> ExistsAsync(int id)
        => await _context.Tests.AnyAsync(test => test.Id == id);

        public async Task<IEnumerable<Test>> GetAllAsync()
        => await _context.Tests
            .Include(t => t.Lesson)
            .Include(t => t.Questions)
            .ToListAsync();

        public async Task<Test?> GetByIdAsync(int id)
            => await _context.Tests
                .Include(t => t.Lesson)
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<Test>> GetByLessonAsync(int lessonId)
        => await _context.Tests
            .Where(t => t.LessonId == lessonId)
            .Include(t => t.Lesson)
            .Include(t => t.Questions)
            .ToListAsync();

        public async Task<Test> UpdateAsync(Test test)
        {
            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
            return test;
        }
    }
}
