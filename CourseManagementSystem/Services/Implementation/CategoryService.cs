using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Category;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;

namespace CourseManagementSystem.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<CategoryService> logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger) 
        { 
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

        public async Task<CategoryResponseDTO> CreateAsync(CategoryCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if (await categoryRepository.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException($"Kategorija {dto.Name} vec postoji!");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Naziv kategorije je obavezan.", nameof(dto.Name));


            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var created = await categoryRepository.CreateAsync(category);
            if (created == null)
            {
                throw new InvalidOperationException("Kategorija nije kreirana.");
            }

            logger.LogInformation("Kreirana kategorija: {Name} | ID: {Id}", created.Name, created.Id);

            return ToResponseDTO(created);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Kategorija sa ID: {id} nije Pronadena");

            if (category.Courses.Any())
                throw new InvalidOperationException(        
                    "Nije moguće obrisati kategoriju koja sadrži tečajeve.");
            
            logger.LogInformation("Obrisana kategorija: ID {Id} | Ime: {Name}", id, category.Name);
            
            return await categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            return categories.Select(ToResponseDTO);
        }

        

        public async Task<CategoryResponseDTO?> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Kategorija sa ID: {id} nije Pronadena");
            return ToResponseDTO(category);
        }

        public async Task<CategoryResponseDTO> UpdateAsync(int id, CategoryUpdateDTO dto)
        {
            var category = await categoryRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Kategorija sa ID: {id} nije Pronadena");

            if (dto.Name is not null)
            {
                if (await categoryRepository.ExistsByNameAsync(dto.Name))
                    throw new InvalidOperationException(
                        $"Kategorija s imenom '{dto.Name}' već postoji.");


                category.Name = dto.Name;
            }

            if (dto.Description is not null) 
            {
                category.Description = dto.Description;
            }

            var updated = await categoryRepository.UpdateAsync(category);

            logger.LogInformation("Ažurirana kategorija: ID {Id} | Novo ime: {Name}", id, category.Name);

            return ToResponseDTO(updated);
        }

        private static CategoryResponseDTO ToResponseDTO(Category category) => new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CoursesCount = category.Courses.Count
        };
        
    }
}
