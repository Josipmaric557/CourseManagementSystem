using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Course;
using CourseManagementSystem.DTOs.UserCourse;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Implementation;
using CourseManagementSystem.Repository.Interfaces;

namespace CourseManagementSystem.Services.Implementation
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUserCourseRepository userCourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ILogger<UserCourseService> logger;

        public UserCourseService(IUserCourseRepository userCourseRepository, ICourseRepository courseRepository
        , ILogger<UserCourseService> logger) 
        { 
            this.userCourseRepository = userCourseRepository;
            this.courseRepository = courseRepository;
            this.logger = logger;
        }

        public async Task<UserCourseResponseDTO> CreateAsync(int userId, UserCourseCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            var Course = await courseRepository.GetByIdAsync(dto.CourseId)
            ?? throw new KeyNotFoundException($"Tečaj s ID-om {dto.CourseId} nije pronađen.");


            if (!Course.IsPublished)
                throw new InvalidOperationException("Ne možeš se prijaviti na neobjavljeni tečaj.");

            if (await userCourseRepository.ExistsAsync(userId, dto.CourseId))
                throw new InvalidOperationException("Već ste prijavljeni na ovaj tečaj.");

            var userCourse = new UserCourse
            {
                UserId = userId,
                CourseId = dto.CourseId,
                StartedAt = dto.StartedAt,
                Status = dto.Status,
                ProgressPercent = 0
            };

            var created = await userCourseRepository.CreateAsync(userCourse)
                          ?? throw new InvalidOperationException("Prijava na tečaj nije kreirana.");
            
            logger.LogInformation("Korisnik ID: {UserId} prijavljen na tečaj ID: {CourseId}", 
                userId, dto.CourseId);

            return toResponseDTO(created);

        }

        public async Task<bool> DeleteAsync(int userId, int courseId)
        {
            var userCourse = await userCourseRepository.GetByUserAndCourseAsync(userId, courseId)
            ?? throw new KeyNotFoundException("Prijava na tečaj nije pronađena.");

            if (userCourse.Status == StartedStatus.Completed)
                throw new InvalidOperationException("Ne možeš se odjaviti s završenog tečaja.");
            
            logger.LogInformation("Korisnik ID: {UserId} odjavljen s tečaja ID: {CourseId}", 
                userId, courseId);

            return await userCourseRepository.DeleteAsync(userId, courseId);
            
        }

        public async Task<IEnumerable<UserCourseResponseDTO>> GetByCourseAsync(int courseId)
        {
            if (!await courseRepository.ExistsAsync(courseId))
                throw new KeyNotFoundException($"Tečaj s ID-om {courseId} nije pronađen.");

            var userCourses = await userCourseRepository.GetByCourseAsync(courseId);
            return userCourses.Select(toResponseDTO);
        }

        public async Task<UserCourseResponseDTO?> GetByUserAndCourseAsync(int userId, int courseId)
        {
            var userCourse = await userCourseRepository.GetByUserAndCourseAsync(userId, courseId);

            if (userCourse is null)
                return null;

            return toResponseDTO(userCourse);
        }

        public async Task<IEnumerable<UserCourseResponseDTO>> GetByUserAsync(int userId)
        {
            var userCourses = await userCourseRepository.GetByUserAsync(userId)
                ?? throw new KeyNotFoundException("Nema usera sa tim id");
            return userCourses.Select(toResponseDTO);
        }

        public async Task<UserCourseResponseDTO> UpdateAsync(int userId, int courseId, UserCourseUpdateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            var userCourse = await userCourseRepository.GetByUserAndCourseAsync(userId, courseId)
            ?? throw new KeyNotFoundException("Prijava na tečaj nije pronađena.");
            
            
            if (dto.ProgressPercent > 0)
            {
                userCourse.ProgressPercent = dto.ProgressPercent;

                
                if (userCourse.ProgressPercent == 100)
                {
                    userCourse.Status = StartedStatus.Completed;
                    userCourse.CompletedAt = DateTime.UtcNow;
                }
            }

            if (dto.Status is not null)
                userCourse.Status = Enum.Parse<StartedStatus>(dto.Status);

            var updated = await userCourseRepository.UpdateAsync(userCourse);
            
            logger.LogInformation("Napredak ažuriran: Korisnik ID: {UserId} | Tečaj ID: {CourseId} | Progres: {Progress}%", 
                userId, courseId, dto.ProgressPercent);

            return toResponseDTO(updated);
        }

        private static UserCourseResponseDTO toResponseDTO(UserCourse userCourse) => new()
        {
            Id = userCourse.Id,
            StartedAt = userCourse.StartedAt,
            CompletedAt = userCourse.CompletedAt,
            Status = userCourse.Status,
            ProgressPercent = userCourse.ProgressPercent,
            UserId = userCourse.UserId,
            UserName = userCourse.User.Username,
            CourseId = userCourse.CourseId,
            CourseTitle = userCourse.Course.Title
        };
    }
}
