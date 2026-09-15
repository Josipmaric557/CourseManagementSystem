using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.DTOs.TestQuestion;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class TestQuestionRepository : ITestQuestionRepository
    {
        private readonly AppDbContext _context;
        public TestQuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestQuestion>> GetAllAsync()
        => await _context.TestQuestions
                .Include(tq => tq.Test)
                .ToListAsync();
        

        public async Task<TestQuestion> CreateAsync(TestQuestion testQuestion)
        {
            _context.TestQuestions.Add(testQuestion);
            await _context.SaveChangesAsync();
            return await _context.TestQuestions
                .Include(tq => tq.Test)
                .FirstAsync(tq => tq.Id == testQuestion.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var testQuestion = await GetByIdAsync(id);

            if (testQuestion is null) return false;

            _context.TestQuestions.Remove(testQuestion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        => await _context.TestQuestions.AnyAsync(tq => tq.Id == id);

        public async Task<TestQuestion?> GetByIdAsync(int id)
        => await _context.TestQuestions
            .Include(tq => tq.Test)
            .FirstOrDefaultAsync(tq => tq.Id == id);

        public async Task<IEnumerable<TestQuestion>> GetByTestAsync(int testId)
        => await _context.TestQuestions
            .Include(tq => tq.Test)
            .Where(tq => tq.Test.Id == testId)
            .ToListAsync();

        public async Task<TestQuestion> UpdateAsync(TestQuestion testQuestion)
        { 
            _context.TestQuestions.Update(testQuestion);
            await _context.SaveChangesAsync();
            return testQuestion;
        }
    }
}
