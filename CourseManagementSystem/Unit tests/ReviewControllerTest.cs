using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Review;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class ReviewControllerTest
{
    private readonly Mock<IReviewService> mockService;
    private readonly ReviewController controller;

    public ReviewControllerTest()
    {
        mockService = new Mock<IReviewService>();
        controller = new ReviewController(mockService.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnOk_WhenCreated()
    {
        var dto = new ReviewCreateDTO {Rating = 5, Comment = "Description"};
        var response = new ReviewResponseDTO {Id = 1, Rating = 5, Comment = "Description"};

        mockService.Setup(s => s.CreateAsync(1, dto)).ReturnsAsync(response);
        
        var result = await controller.Create(1, dto);

        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<ReviewResponseDTO>().Subject;
        returned.Comment.Should().Be("Description");
    }

    [Fact]
    public async Task CreateAsync_ThrowsKeyNotFoundException_WhenCourseNotFound()
    {
        var dto = new ReviewCreateDTO {Rating = 5, Comment = "Description", CourseId = 1};

        mockService.Setup(s => s.CreateAsync(1, dto)).ThrowsAsync(new KeyNotFoundException("nepostoji course sa tim id-om"));

        var act = async () => await controller.Create(1, dto);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji course sa tim id-om");
    
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1, 2));

        var result = await controller.Delete(1, 2);
        
        result.Should().BeOfType<NoContentResult>();
    
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenReviewNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1, 2))
            .ThrowsAsync(new KeyNotFoundException("Review sa tim id ne postoji"));

        var act = async () => await controller.Delete(1, 2);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Review sa tim id ne postoji");
    
    }

    [Fact]
    public async Task DeleteAsync_ThrowsUnautherizedAccessException_WhenAtemptingDeletingAnotherUserReview()
    {
        mockService
            .Setup(s => s.DeleteAsync(1, 1))
            .ThrowsAsync(new UnauthorizedAccessException("Ne možete brisati tuđu recenziju."));

        var act = async () => await controller.Delete(1, 1);

        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Ne možete brisati tuđu recenziju.");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOk_WithListOfReviews()
    {
       
        var reviews = new List<ReviewResponseDTO>
        {
            new() {Id = 1, Rating = 5, Comment = "Description"},
            new() {Id = 2, Rating = 5, Comment = "Description"},
        };

        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(reviews);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<ReviewResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkWithEmptyList_WhenNoReviews()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ReviewResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<ReviewResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByCoursesAsync_ReturnsListOfReviews_WhenCoursesExists()
    {
        var reviews = new List<ReviewResponseDTO>
        {
            new() {Id = 1, Rating = 5, Comment = "Description", CourseName = "Course"},
            new() {Id = 2, Rating = 5, Comment = "Description", CourseName = "Course"},
        };

        mockService.Setup(s => s.GetByCourses(1))
            .ReturnsAsync(reviews);

        var result = await controller.GetByCourses(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<ReviewResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(c => c.CourseName == "Course").Should().BeTrue();
    
    }

    [Fact]
    public async Task GetByCoursesAsync_ThrowKeyNotFoundException_WhenCourseNotFound()
    {
        mockService.Setup(s=> s.GetByCourses(100))
            .ThrowsAsync(new KeyNotFoundException("Kategorija sa id nije pronadena"));

        var act = async () => await controller.GetByCourses(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("kategorija sa id nije pronadena");

    }

    [Fact]
    public async Task GetByIdAsync_RetursListOfreviews_WhenReviewExists()
    {
        var review = new ReviewResponseDTO {Id = 1, Rating = 5, Comment = "Description"};

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(review);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<ReviewResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Comment.Should().Be("Description");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenReviewNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Review sa tim id nije pronaden"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Review sa tim id nije pronaden");
    }

    [Fact]
    public async Task GetByUserAndCourseAsync_ReturnsReview_WhenReviewExists()
    {
        var review1 = new ReviewResponseDTO
        {
            Rating = 5,
            Comment = "Description"
        };

        mockService
            .Setup(s => s.GetByUserAndCourseAsync(1, 1))
            .ReturnsAsync(review1);

        var result = await controller.GetByUserAndCourse(1, 1);

        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        var returned = okResult.Value.Should()
            .BeOfType<ReviewResponseDTO>()
            .Subject;

        returned.Rating.Should().Be(5);
        returned.Comment.Should().Be("Description");
    }

    [Fact]
    public async Task GetByUserAndCourseAsync_ReturnsNotFound_WhenReviewDoesNotExist()
    {
        mockService.Setup(s => s.GetByUserAndCourseAsync(1, 999))
            .ReturnsAsync((ReviewResponseDTO?)null);

        
        var result = await controller.GetByUserAndCourse(1, 999);

        
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsOk_WhenUserExists()
    {
        var reviews = new List<ReviewResponseDTO>
        {
            new() {Id = 1, Rating = 5, Comment = "Description", CourseName = "Course", UserName = "User"},
            new() {Id = 2, Rating = 5, Comment = "Description", CourseName = "Course" , UserName = "User"},
        };

        mockService.Setup(s => s.GetByUsers(1))
            .ReturnsAsync(reviews);

        var result = await controller.GetByUsers(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<ReviewResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(c => c.UserName == "User").Should().BeTrue();
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsOK_WithEmptyList_WhenNoUsers()
    {
        mockService.Setup(s => s.GetByUsers(1))
            .ReturnsAsync(new List<ReviewResponseDTO>());

        var result = await controller.GetByUsers(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<ReviewResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenValid()
    {
        var dto = new ReviewUpdateDTO {Rating = 5, Comment = "Description", CourseId = 1 , UserId= 1};
        var response = new ReviewResponseDTO {Id = 1, Rating = 5, Comment = "Description", CourseName = "Course" , UserName = "User"};

        mockService.Setup(s => s.UpdateAsync(1, 1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1, 1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<ReviewResponseDTO>().Subject;
        returned.Rating.Should().Be(5);
        returned.Comment.Should().Be("Description");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsKeyNotFoundException_WhenReviewDoesNotExist()
    {
        var dto = new ReviewUpdateDTO {Rating = 5, Comment = "Description", CourseId = 1 , UserId= 1};
        mockService.Setup(s => s.UpdateAsync(1,1, dto))
            .ThrowsAsync(new KeyNotFoundException("Recenzija sa tim id nije pronadena"));

        var act = async () => await controller.Update(1, 1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Recenzija sa tim id nije pronadena");
    
    }

        [Fact]
        public async Task UpdateAsync_ThrowsUnauthorizedAccessException_WhenUserTryEraseSomeoneElsesReview()
        {
            var dto = new ReviewUpdateDTO {Rating = 5, Comment = "Description", CourseId = 1 , UserId= 1};
            mockService.Setup(s => s.UpdateAsync(1,1, dto))
                .ThrowsAsync(new UnauthorizedAccessException("Nemozete brisat tudu recenziju"));

            var act = async () => await controller.Update(1, 1,dto);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Nemozete brisat tudu recenziju");
        }

        [Fact]
        public async Task UpdateAsync_ThrowsInvalidOperationException_WhenRatingIsNotBetween1_5()
        {
            
            var dto = new ReviewUpdateDTO {Rating = 5, Comment = "Description", CourseId = 1 , UserId= 1};

            mockService
                .Setup(s => s.UpdateAsync(1, 1, dto))
                .ThrowsAsync(new InvalidOperationException("Ocjena mora biti između 1 i 5."));

            var act = async () => await controller.Update(1, 1, dto);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Ocjena mora biti između 1 i 5.");
        }
}