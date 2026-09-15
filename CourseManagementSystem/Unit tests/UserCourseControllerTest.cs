using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.DTOs.UserCourse;
using CourseManagementSystem.Services.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class UserCourseControllerTest
{
    private readonly Mock<IUserCourseService> mockService;
    private readonly UserCourseController controller;

    public UserCourseControllerTest()
    {
        mockService = new Mock<IUserCourseService>();
        controller = new UserCourseController(mockService.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnsOk_WhenCreated()
    {
        var dto = new UserCourseCreateDTO { ProgressPercent = 30, CourseId = 2};
        var response =  new UserCourseResponseDTO { Id = 3, ProgressPercent = 30, CourseId = 2 };
        
        mockService.Setup(s => s.CreateAsync(1, dto)).ReturnsAsync(response);
        
        var result = await controller.Create(1, dto);
        
        
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<UserCourseResponseDTO>().Subject;
        returned.ProgressPercent.Should().Be(30);

    }

    [Fact]
    public async Task CreateAsync_ThrowsKeyNotFoundException_WhenCourseNotFound()
    {
        var dto = new UserCourseCreateDTO { ProgressPercent = 30, CourseId = 2};

        mockService.Setup(s => s.CreateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji course"));

        var act = async () => await controller.Create(1, dto);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji course");

    }

    [Fact]
    public async Task CreateAsync_ThrowsInvalidOperationException_WhenCourseNotPublishedYet()
    {
        var dto = new UserCourseCreateDTO 
        { 
            ProgressPercent = 30, 
            CourseId = 2
        };

        mockService
            .Setup(s => s.CreateAsync(1, dto))
            .ThrowsAsync(new InvalidOperationException("Ne možeš se prijaviti na neobjavljeni tečaj."));

        var act = async () => await controller.Create(1, dto);
        
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Ne možeš se prijaviti na neobjavljeni tečaj.");
    }

    [Fact]
    public async Task CreateAsync_ThrowsInvalidOperationExcpetion_WhenUserAlreadyIsLoggedIn()
    {
        var dto = new UserCourseCreateDTO 
        { 
            ProgressPercent = 30, 
            CourseId = 2
        };

        mockService
            .Setup(s => s.CreateAsync(1, dto))
            .ThrowsAsync(new InvalidOperationException("Već ste prijavljeni na ovaj tečaj."));

        var act = async () => await controller.Create(1, dto);
        
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Već ste prijavljeni na ovaj tečaj.");
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1,1)).ReturnsAsync(true);
        
        var result = await controller.Delete(1, 1);

        result.Should().BeOfType<NoContentResult>();

    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenLoginNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1,1))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji lekcija koju trazite"));

        var act = async () => await controller.Delete(1,1);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji lekcija koju trazite");

    }

    [Fact]
    public async Task DeleteAsync_ThrowsInvalidOperationException_WhenTryToLogoutFromAlreadyFinishedCourse()
    {
        mockService.Setup(s => s.DeleteAsync(1,1))
            .ThrowsAsync(new InvalidOperationException("Nepostoji lekcija koju trazite"));

        var act = async () => await controller.Delete(1,1);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Nepostoji lekcija koju trazite");

    }

    [Fact]
    public async Task GetByCourseAsync_ReturnsOk_WithListOfUserCourses()
    {
        var userCourse = new List<UserCourseResponseDTO>
        {
            new() { Id = 1, ProgressPercent = 30, UserId = 1},

            new() { Id = 1, ProgressPercent = 50, UserId = 1}

        };

        mockService.Setup(s => s.GetByCourseAsync(1))
            .ReturnsAsync(userCourse);

        var result = await controller.GetByCourse(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<UserCourseResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(l => l.UserId == 1).Should().BeTrue();

    }

    [Fact]
    public async Task GetByCoursesAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        mockService.Setup(s=> s.GetByCourseAsync(100))
            .ThrowsAsync(new KeyNotFoundException("UserCourse nemaju takav course"));

        var act = async () => await controller.GetByCourse(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("UserCourse nemaju takav course");
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsOk_WithListOfCourses()
    {
        var userCourse = new List<UserCourseResponseDTO>
        {
            new() { Id = 1, ProgressPercent = 30, UserId = 1},

            new() { Id = 1, ProgressPercent = 50, UserId = 1}

        };

        mockService.Setup(s => s.GetByCourseAsync(1))
            .ReturnsAsync(userCourse);

        var result = await controller.GetByCourse(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<UserCourseResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(l => l.UserId == 1).Should().BeTrue();

    }

    [Fact]
    public async Task GetByUserAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        mockService.Setup(s=> s.GetByUserAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Useri nemaju takav course"));

        var act = async () => await controller.GetByUser(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Useri nemaju takav course");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var dto = new UserCourseUpdateDTO { ProgressPercent = 30, Status = "Odlican" };
        var response =  new UserCourseResponseDTO { Id = 3, ProgressPercent = 30, CourseId = 2};
        
        mockService.Setup(s => s.UpdateAsync(1, 1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1,1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<UserCourseResponseDTO>().Subject;
        returned.ProgressPercent.Should().Be(30);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        var dto = new UserCourseUpdateDTO { ProgressPercent = 30, Status = "Odlican" };
        mockService.Setup(s => s.UpdateAsync(1,1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji userCourse sa tim id-om"));

        var act = async () => await controller.Update(1,1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji userCourse sa tim id-om");
    }
}

