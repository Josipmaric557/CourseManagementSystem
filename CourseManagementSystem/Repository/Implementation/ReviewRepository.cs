using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Course)
                .FirstAsync(r => r.Id == review.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await GetByIdAsync(id);

            if (review is null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByUserAndCourseAsync(int UserId, int CourseId)
        => await _context.Reviews.AnyAsync(r => r.UserId == UserId && r.CourseId == CourseId);


        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByCourses(int CourseId)
        => await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Course)
            .Where(r => r.CourseId == CourseId)
            .ToListAsync();

        public async Task<Review?> GetByIdAsync(int id)
        => await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Course)
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Review?> GetByUserAndCourseAsync(int UserId, int CourseId)
        => await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Course)
            .FirstOrDefaultAsync(r => r.UserId == UserId && r.CourseId == CourseId);


        public async Task<IEnumerable<Review>> GetByUsers(int UserId)
        => await _context.Reviews
            .Include(r => r.Course)
            .Include(r => r.User)
            .Where(r => r.UserId == UserId)
            .ToListAsync();


        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Course)
                .FirstAsync(r => r.Id == review.Id);
        }
    }
}
