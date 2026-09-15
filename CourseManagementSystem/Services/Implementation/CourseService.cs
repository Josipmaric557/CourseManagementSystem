using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Course;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Implementation;
using CourseManagementSystem.Repository.Interfaces;

namespace CourseManagementSystem.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<CourseService> logger;

        public CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository, ILogger<CourseService> logger) 
        {
            this.courseRepository = courseRepository;
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }
        public async Task<CourseResponseDTO> CreateAsync(CourseCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if (!await categoryRepository.ExistsById(dto.CategoryId))
                throw new KeyNotFoundException($"Kategorija s ID-om {dto.CategoryId} nije pronađena.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Naziv tečaja je obavezan.", nameof(dto.Title));

            if (dto.Price < 0)
                throw new ArgumentOutOfRangeException(nameof(dto.Price), "Cijena ne može biti negativna.");


            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                IsPublished = dto.isPublihed
            };

            var created = await courseRepository.CreateAsync(course) 
                          ?? throw new InvalidOperationException("Tečaj nije uspješno kreiran.");
            
            logger.LogInformation("Kreiran tečaj: {Title} | ID: {Id} | Kategorija: {CategoryId}", 
                created.Title, created.Id, created.CategoryId);
            
            return ToResponseDTO(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await courseRepository.GetByIdAsync(id) 
                         ?? throw new KeyNotFoundException($"Tečaj s ID-om {id} nije pronađen.");
            if (course.UserCourses.Any()) 
                throw new InvalidOperationException("Nije moguće obrisati tečaj koji ima prijavljene korisnike.");
            logger.LogInformation("Obrisan tečaj: ID {Id} | Naziv: {Title}", id, course.Title);
            
            return await courseRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetAllAsync()
        {
            var courses = await courseRepository.GetAllAsync();

            return courses.Select(ToResponseDTO);
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetByCategoryAsync(int CategoryId)
        {
            if (!await categoryRepository.ExistsById(CategoryId)) 
                throw new KeyNotFoundException($"Kategorija s ID-om {CategoryId} nije pronađena!");

            var courses = await courseRepository.GetByCategoryAsync(CategoryId);
            return courses.Select(ToResponseDTO);
        }

        public async Task<CourseResponseDTO?> GetByIdAsync(int id)
        {
            var course = await courseRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Tečaj s ID-om {id} nije pronađen.");

            return ToResponseDTO(course);

        }

        public async Task<CourseResponseDTO> UpdateAsync(int id, CourseUpdateDTO dto)
        {
            var course = await courseRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Tečaj s ID-om {id} nije pronađen.");

            if (dto.Title is not null)
                course.Title = dto.Title;

            if(dto.Description is not null)
                course.Description = dto.Description;

            if (dto.CategoryId is not null) 
            {
                if (!await categoryRepository.ExistsById(dto.CategoryId.Value))
                    throw new KeyNotFoundException($"Kategorija s ID-om {dto.CategoryId} nije pronađena.");

                course.CategoryId = dto.CategoryId.Value;

            }

            var updated = await courseRepository.UpdateAsync(course)
                          ?? throw new InvalidOperationException("Ažuriranje tečaja nije uspjelo.");
            
            
            logger.LogInformation("Ažuriran tečaj: ID {Id} | Naziv: {Title}", id, course.Title);

            return ToResponseDTO(updated);

        }

        private static CourseResponseDTO ToResponseDTO(Course course)
        => new()
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            IsPublished = course.IsPublished,
            CreatedAt = course.CreatedAt,
            CategoryId = course.CategoryId,
            CategoryName = course.Category?.Name ?? string.Empty,
            UserRoleCounts = course.UserCourses.Count,
            LessonsCount = course.Lessons.Count,
            ReviewsCount = course.Reviews.Count


        };
    }
}
