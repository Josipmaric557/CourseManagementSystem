using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.TestQuestion;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Implementation;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CourseManagementSystem.Services.Implementation
{
    public class TestQuestionService : ITestQuestionService
    {
        private readonly ITestQuestionRepository testQuestionRepository;
        private readonly ITestRepository testRepository;
        private readonly ILogger<TestQuestionService> logger;

        public TestQuestionService(ITestQuestionRepository testQuestionRepository, ITestRepository testRepository
        , ILogger<TestQuestionService> logger) 
        {
            this.testQuestionRepository = testQuestionRepository;
            this.testRepository = testRepository;
            this.logger = logger;
        }
        public async Task<TestQuestionResponseDTO> CreateAsync(TestQuestionCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if(!await testRepository.ExistsAsync(dto.TestId))
                throw new KeyNotFoundException($"Test s ID-om {dto.TestId} nije pronađen.");
            
            if (string.IsNullOrWhiteSpace(dto.Question))
                throw new ArgumentException(
                    "Tekst pitanja je obavezan.",
                    nameof(dto.Question));
            if (dto.Points < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(dto.Points),
                    "Broj bodova ne može biti negativan.");
            if (string.IsNullOrWhiteSpace(dto.CorrectAnswer))
                throw new ArgumentException(
                    "Točan odgovor je obavezan.",
                    nameof(dto.CorrectAnswer));



            var testQuestin = new TestQuestion
            {
                Question = dto.Question,
                OptionA = dto.OptionA,
                OptionB = dto.OptionB,
                OptionC = dto.OptionC,
                CorrectAnswer = dto.CorrectAnswer,
                Points = dto.Points,
                TestId = dto.TestId,
            };

            var created = await testQuestionRepository.CreateAsync(testQuestin)
                          ?? throw new InvalidOperationException("Pitanje nije uspješno kreirano.");
            
            logger.LogInformation("Kreiran pitanje: ID {Id} | Test ID: {TestId}", 
                created.Id, created.TestId);
            
            return toResponseDTO(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var testQuestion = await testQuestionRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Ne postoji question sa tim id:{id}");

            logger.LogInformation("Obrisano pitanje: ID {Id} | Test ID: {TestId}", id, testQuestion.TestId);
            
            return await testQuestionRepository.DeleteAsync(id);

        }

        public async Task<TestQuestionResponseDTO?> GetByIdAsync(int id)
        {
            var question = await testQuestionRepository.GetByIdAsync(id)
           ?? throw new KeyNotFoundException($"Pitanje s ID-om {id} nije pronađeno.");

            return toResponseDTO(question);
        }

        public async Task<IEnumerable<TestQuestionResponseDTO>> GetAllAsync()
        {
            var questions = await testQuestionRepository.GetAllAsync();
            return questions.Select(toResponseDTO);
        }

        public async Task<IEnumerable<TestQuestionResponseDTO>> GetByTestAsync(int testId)
        {
            if(!await testRepository.ExistsAsync(testId))
                throw new KeyNotFoundException($"Test s ID-om {testId} nije pronađen.");

            var testQuestions = await testQuestionRepository.GetByTestAsync(testId);
            return testQuestions.Select(toResponseDTO);
        }

        public async Task<TestQuestionResponseDTO> UpdateAsync(int id, TestQuestionUpdateDTO dto)
        {
            var testQuestion = await testQuestionRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Pitanje s ID-om {id} nije pronađeno.");
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));


            if (dto.Question is not null)
                testQuestion.Question = dto.Question;

            if (dto.OptionA is not null)
                testQuestion.OptionA = dto.OptionA;

            if (dto.OptionB is not null)
                testQuestion.OptionB = dto.OptionB;

            if (dto.OptionC is not null)
                testQuestion.OptionC = dto.OptionC;

            if (dto.CorrectAnswer is not null)
                testQuestion.CorrectAnswer = dto.CorrectAnswer;

            if (dto.Points.HasValue)
            {
                if (dto.Points.Value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(dto.Points));

                testQuestion.Points = dto.Points.Value;
            }


            var updated = await testQuestionRepository.UpdateAsync(testQuestion)
                          ?? throw new InvalidOperationException("Ažuriranje pitanja nije uspjelo.");
            
            logger.LogInformation("Ažurirano pitanje: ID {Id}", id);
            
            return toResponseDTO(updated);

        }
        private static TestQuestionResponseDTO toResponseDTO(TestQuestion testQuestion) => new()
        {
            Id = testQuestion.Id,
            Question = testQuestion.Question,
            OptionA = testQuestion.OptionA,
            OptionB = testQuestion.OptionB,
            OptionC = testQuestion.OptionC,
            CorrectAnswer = testQuestion.CorrectAnswer,
            Points = testQuestion.Points,
            TestId = testQuestion.TestId,
            TestName = testQuestion.Test.Title
        };
    }
}
