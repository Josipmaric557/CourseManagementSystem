using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.DTOs.Review;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using CourseManagementSystem.Services.Interfaces;

namespace CourseManagementSystem.Services.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IUserCourseRepository userCourseRepository;
        private readonly ILogger<ReviewService> logger;

        public ReviewService(
            IReviewRepository reviewRepository,
            ICourseRepository courseRepository,
            IUserCourseRepository userCourseRepository, 
            ILogger<ReviewService> logger)
        {
            this.reviewRepository = reviewRepository;
            this.courseRepository = courseRepository;
            this.userCourseRepository = userCourseRepository;
            this.logger = logger;
        }

        public async Task<ReviewResponseDTO> CreateAsync(int userId, ReviewCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (!await courseRepository.ExistsAsync(dto.CourseId))
                throw new KeyNotFoundException($"Tečaj s ID-om {dto.CourseId} nije pronađen.");

            if (!await userCourseRepository.ExistsAsync(userId, dto.CourseId))
                throw new InvalidOperationException("Morate biti prijavljeni na tečaj da biste ostavili recenziju.");

            if (await reviewRepository.ExistsByUserAndCourseAsync(userId, dto.CourseId))
                throw new InvalidOperationException("Već ste ostavili recenziju za ovaj tečaj.");

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Ocjena mora biti između 1 i 5.");
            

            var review = new Review
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = userId,
                CourseId = dto.CourseId
            };

            var created = await reviewRepository.CreateAsync(review)
                          ?? throw new InvalidOperationException("Recenzija nije kreirana.");
            
            logger.LogInformation("Kreirana recenzija: ID {Id} | Korisnik ID: {UserId} | Tečaj ID: {CourseId} | Ocjena: {Rating}", 
                created.Id, userId, dto.CourseId, dto.Rating);

            
            return ToResponseDTO(created);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var review = await reviewRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Recenzija s ID-om {id} nije pronađena.");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException("Ne možete brisati tuđu recenziju.");

            logger.LogInformation("Obrisana recenzija: ID {Id} | Korisnik ID: {UserId}", id, userId);
            
            return await reviewRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetAllAsync()
        {
            var reviews = await reviewRepository.GetAllAsync();
            return reviews.Select(ToResponseDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetByCourses(int courseId)
        {
            if (!await courseRepository.ExistsAsync(courseId))
                throw new KeyNotFoundException($"Tečaj s ID-om {courseId} nije pronađen.");

            var reviews = await reviewRepository.GetByCourses(courseId);
            return reviews.Select(ToResponseDTO);
        }

        public async Task<ReviewResponseDTO?> GetByIdAsync(int id)
        {
            var review = await reviewRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Recenzija s ID-om {id} nije pronađena.");

            return ToResponseDTO(review);
        }

        public async Task<ReviewResponseDTO?> GetByUserAndCourseAsync(int userId, int courseId)
        {
            var review = await reviewRepository.GetByUserAndCourseAsync(userId, courseId);

            if (review is null)
                return null;

            return ToResponseDTO(review);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetByUsers(int userId)
        {
            var reviews = await reviewRepository.GetByUsers(userId);
            return reviews.Select(ToResponseDTO);
        }

        public async Task<ReviewResponseDTO> UpdateAsync(int id, int userId, ReviewUpdateDTO dto)
        {
            var review = await reviewRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Recenzija s ID-om {id} nije pronađena.");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException("Ne možete mijenjati tuđu recenziju.");

            if (dto.Rating is not null)
            {
                if (dto.Rating < 1 || dto.Rating > 5)
                    throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Ocjena mora biti između 1 i 5.");
                review.Rating = dto.Rating.Value;
            }

            if (dto.Comment is not null)
                review.Comment = dto.Comment;

            var updated = await reviewRepository.UpdateAsync(review)
                          ?? throw new InvalidOperationException("Ažuriranje recenzije nije uspjelo.");
            
            logger.LogInformation("Ažurirana recenzija: ID {Id} | Korisnik ID: {UserId}", id, userId);

            return ToResponseDTO(updated);
        }

        private static ReviewResponseDTO ToResponseDTO(Review review) => new()
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            UserName = review.User.Username,
            CourseName = review.Course.Title
        };
    }
}