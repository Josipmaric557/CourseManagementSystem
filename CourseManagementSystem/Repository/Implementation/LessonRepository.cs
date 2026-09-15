using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;
        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return await _context.Lessons
                .Include(l => l.Course)
                .FirstAsync(l => l.Id == lesson.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lesson = await GetByIdAsync(id);

            if (lesson is null) return false;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(int id)
        => _context.Lessons.AnyAsync(l => l.Id == id);

        public async Task<IEnumerable<Lesson>> GetAllAsync()
        => _context.Lessons
            .Include(l => l.Course)
            .ToList();

        public async Task<IEnumerable<Lesson>> GetByCourseAsync(int Courseid)
        => await _context.Lessons
            .Include(l => l.Course)
            .Where(l => l.CourseId == Courseid)
            .ToListAsync();

        public async Task<Lesson?> GetByIdAsync(int id)
        => await _context.Lessons
            .Include(l => l.Course)
            .FirstOrDefaultAsync(l => l.Id == id);

        public async Task<Lesson> UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();

            return await _context.Lessons
                .Include(l => l.Course)
                .FirstAsync(l => l.Id == lesson.Id);
        }
    }
}
