using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Implementation;
using CourseManagementSystem.Repository.Interfaces;

namespace CourseManagementSystem.Services.Implementation
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository lessonRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ILogger<LessonService> logger;

        public LessonService(ILessonRepository lessonRepository, ICourseRepository courseRepository, ILogger<LessonService> logger) 
        {
            this.lessonRepository = lessonRepository;
            this.courseRepository = courseRepository;
            this.logger = logger;
        }
        public async Task<LessonResponseDTO> CreateAsync(LessonCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if(!await courseRepository.ExistsAsync(dto.CourseId))
                throw new KeyNotFoundException($"Tečaj s ID-om {dto.CourseId} nije pronađen.");
            
            

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Naziv lekcije je obavezan.", nameof(dto.Title));

            if (dto.DurationMinutes < 0)
                throw new ArgumentOutOfRangeException(nameof(dto.DurationMinutes));

            if (dto.OrderIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(dto.OrderIndex));


            var lesson = new Lesson
            {
                Title = dto.Title,
                Content = dto.Content,
                OrderIndex = dto.OrderIndex,
                DurationMinutes = dto.DurationMinutes,
                CourseId = dto.CourseId,
            };

            var created = await lessonRepository.CreateAsync(lesson)
                          ?? throw new InvalidOperationException("Lekcija nije kreirana.");
            

            logger.LogInformation("Kreirana lekcija: {Title} | ID: {Id} | Tečaj ID: {CourseId}", 
                created.Title, created.Id, created.CourseId);

            return toResponseDTO(created);
        }

        

        public async Task<bool> DeleteAsync(int id)
        {
            var lesson =  await lessonRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Tečaj s ID-om {id} nije pronađen.");

            logger.LogInformation("Obrisana lekcija: ID {Id} | Naziv: {Title}", id, lesson.Title);

            return await lessonRepository.DeleteAsync(id);


        }

        public async Task<IEnumerable<LessonResponseDTO>> GetAllAsync()
        {
            var lessons = await lessonRepository.GetAllAsync();

            return lessons.Select(toResponseDTO);
        }

        public async Task<IEnumerable<LessonResponseDTO>> GetByCourseAsync(int Courseid)
        {
            if (!await courseRepository.ExistsAsync(Courseid))
                throw new KeyNotFoundException(
                    $"Tečaj s ID-om {Courseid} nije pronađen.");
            var lessons = await lessonRepository.GetByCourseAsync(Courseid);
            return lessons.Select(toResponseDTO);
        }

        public async Task<LessonResponseDTO?> GetByIdAsync(int id)
        {
            var lesson = await lessonRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Nema lesson sa id:{id}");

            return toResponseDTO(lesson);
        }

        public async Task<LessonResponseDTO> UpdateAsync(int id, LessonUpdateDTO dto)
        {
            var lesson = await lessonRepository.GetByIdAsync(id)
               ?? throw new KeyNotFoundException($"Lekcija s ID-om {id} nije pronađena.");

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.Title != null &&
                string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException(
                    "Naziv lekcije ne može biti prazan.",
                    nameof(dto.Title));
            }
            if (dto.DurationMinutes.HasValue &&
                dto.DurationMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dto.DurationMinutes));
            }
            if (dto.OrderIndex.HasValue &&
                dto.OrderIndex < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dto.OrderIndex));
            }
            if (dto.CourseId.HasValue)
            {
                if (!await courseRepository.ExistsAsync(dto.CourseId.Value))
                    throw new KeyNotFoundException(
                        $"Tečaj s ID-om {dto.CourseId.Value} nije pronađen.");

                lesson.CourseId = dto.CourseId.Value;
            }

            

            if(dto.Title is not null) lesson.Title = dto.Title;
            if(dto.Content is not null) lesson.Content = dto.Content;
            if (dto.OrderIndex is not null) lesson.OrderIndex = dto.OrderIndex.Value;
            if (dto.DurationMinutes is not null) lesson.DurationMinutes = dto.DurationMinutes.Value;
            if(dto.CourseId is not null) lesson.CourseId = dto.CourseId.Value;

            var updated = await lessonRepository.UpdateAsync(lesson) 
                          ?? throw new InvalidOperationException("Ažuriranje lekcije nije uspjelo.");
            
            logger.LogInformation("Ažurirana lekcija: ID {Id} | Naziv: {Title}", id, lesson.Title);

            return toResponseDTO(lesson);



        }
        private static LessonResponseDTO toResponseDTO(Lesson lesson) => new()
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            OrderIndex = lesson.OrderIndex,
            DurationMinutes = lesson.DurationMinutes,
            CourseId = lesson.CourseId,
            CourseTitle = lesson.Course?.Title ?? string.Empty,
            TestsCount = lesson.Tests.Count
        };
    }
}
