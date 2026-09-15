using System.Collections.Generic;
using System.Linq;
using CourseManagementSystem.DTOs.Test;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseManagementSystem.Services.Implementation
{
    public class TestService : ITestService
    {
        private readonly ITestRepository testRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly ILogger<TestService> logger;

        public TestService(ITestRepository testRepository, ILessonRepository lessonRepository, ILogger<TestService> logger) 
        {
            this.testRepository = testRepository;
            this.lessonRepository = lessonRepository;
            this.logger = logger;
        }


        public async Task<TestResponseDTO> CreateAsync(TestCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if(!await lessonRepository.ExistsAsync(dto.LessonId))
                throw new KeyNotFoundException($"Lekcija s ID-om {dto.LessonId} nije pronađena.");
            

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException(
                    "Naziv testa je obavezan.",
                    nameof(dto.Title));
            
            if (dto.PassingScore < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(dto.PassingScore),
                    "Bodovi ne mogu biti negativni.");

            
            var test = new Test
            {
                Title = dto.Title,
                PassingScore = dto.PassingScore,
                LessonId = dto.LessonId
            };

            var created = await testRepository.CreateAsync(test)
                          ?? throw new InvalidOperationException("Test nije uspješno kreiran.");
            
            logger.LogInformation("Kreiran test: {Title} | ID: {Id} | Lekcija ID: {LessonId}", 
                created.Title, created.Id, created.LessonId);
            
            return toResponseDTO(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var test = await testRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Test s ID-om {id} nije pronađen.");

            logger.LogInformation("Obrisan test: ID {Id} | Naziv: {Title}", id, test.Title);
            
            return await testRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TestResponseDTO>> GetAllAsync()
        {
            var tests = await testRepository.GetAllAsync();

            return tests.Select(toResponseDTO);
        }

        public async Task<TestResponseDTO?> GetByIdAsync(int id)
        {
            var test = await testRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Tečaj s ID-om {id} nije pronađen.");
            return toResponseDTO(test);

        }

        public async Task<IEnumerable<TestResponseDTO>> GetByLessonAsync(int lessonId)
        {
            if(!await lessonRepository.ExistsAsync(lessonId))
                throw new KeyNotFoundException($"Lekcija s ID-om {lessonId} nije pronađena.");

            var tests = await testRepository.GetByLessonAsync(lessonId);

            return tests.Select(toResponseDTO);
        }

        public async Task<TestResponseDTO> UpdateAsync(int id, TestUpdateDTO dto)
        {
            
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            var test = await testRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Test s ID-om {id} nije pronađen.");

            if(dto.Title is not null)
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    throw new ArgumentException(
                        "Naziv testa ne može biti prazan.",
                        nameof(dto.Title));

                test.Title = dto.Title;
            }


            if(dto.Title is not null) test.Title = dto.Title;
            if (dto.LessonId is not null) test.LessonId = dto.LessonId.Value;

            var updated = await testRepository.UpdateAsync(test)
                          ?? throw new InvalidOperationException("Ažuriranje testa nije uspjelo.");
            
            logger.LogInformation("Ažuriran test: ID {Id} | Naziv: {Title}", id, test.Title);
            
            return toResponseDTO(updated);
        }
        private static TestResponseDTO toResponseDTO(Test test) => new()
        {
            Id = test.Id,
            Title = test.Title,
            PassingScore = test.PassingScore,
            LessonId = test.LessonId,
            LessonTitle = test.Lesson.Title,
            QuestionsCount = test.Questions.Count

        };
    }
}
