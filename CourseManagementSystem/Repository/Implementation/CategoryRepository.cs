using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _contex;
        public CategoryRepository(AppDbContext context)
        {
            _contex = context;
        }

        public async Task<Category> CreateAsync(Category category)
        { 
            _contex.Categories.Add(category);
            await _contex.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category is null) return false;

            _contex.Categories.Remove(category);
            await _contex.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsById(int id)
         => await _contex.Categories.AnyAsync(c => c.Id == id);

        public async Task<bool> ExistsByNameAsync(string name)
        => await _contex.Categories.AnyAsync(c => c.Name == name);

        public async Task<IEnumerable<Category>> GetAllAsync()
        => await _contex.Categories
            .Include(c => c.Courses)
            .ToListAsync();

        public async Task<Category?> GetByIdAsync(int id) =>
            await _contex.Categories
            .Include(c => c.Courses)
            .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category> UpdateAsync(Category category)
        {
            _contex.Categories.Update(category);
            await _contex.SaveChangesAsync();
            return category;
        }
    }
}
