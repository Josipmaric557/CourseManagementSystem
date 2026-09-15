using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Category;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.Services.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class LessonControllerTest
{
    private readonly Mock<ILessonService> mockService;
    private readonly LessonController controller;

    public LessonControllerTest()
    {
        mockService = new Mock<ILessonService>();
        controller = new LessonController(mockService.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnsOk_WhenLessonCreated()
    {
        var dto = new LessonCreateDTO { Title = "Marketing", Content = "Odlican marketing imate!" };
        var response =  new LessonResponseDTO { Id = 3, Title = "Marketing", Content = "Odlican marketing imate!" };
        
        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);
        
        var result = await controller.Create(dto);
        
        
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<LessonResponseDTO>().Subject;
        returned.Title.Should().Be("Marketing");
    
    }

    [Fact]
    public async Task CreateAsync_ThrowsKeyNotFoundException_WhenLessonNotFound()
    {
        var dto = new LessonCreateDTO { Title = "Marketing", Content = "Odlican marketing imate!" };

        mockService.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji lesson"));

        var act = async () => await controller.Create(dto);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji lesson");
    
    }

    [Fact]
    public async Task DeleteAsync_ReturnNoContent_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenLessonNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji lekcija koju trazite"));

        var act = async () => await controller.Delete(1);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji lekcija koju trazite");
    
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOk_WithListOfLessons()
    {
        var lessons = new List<LessonResponseDTO>
        {
            new() { Id = 1, Title = "Marketing", Content = "Odlican marketing imate!" },

            new() { Id = 2, Title = "Dizajn", Content = "Category 2" }

        };
        
        mockService.Setup(s=> s.GetAllAsync()).ReturnsAsync(lessons);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<LessonResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkWithEmptyList_WhenNoLessons()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<LessonResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<LessonResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByCourseAsync_ReturnsListOfLessons_WhenLessonExist()
    {
        var lessons = new List<LessonResponseDTO>
        {
            new() { Id = 1, Title = "Marketing", Content = "Odlican marketing imate!", CourseId = 1},

            new() { Id = 2, Title = "Dizajn", Content = "Category 2", CourseId = 1 }

        };

        mockService.Setup(s => s.GetByCourseAsync(1))
            .ReturnsAsync(lessons);

        var result = await controller.GetByCourse(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<LessonResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(l => l.CourseId == 1).Should().BeTrue();
    
    }

    [Fact]
    public async Task GetByCourseAsync_ThrowsKeyNotFoundException_WhenLessonNotFound()
    {
        mockService.Setup(s=> s.GetByCourseAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Lessoni nemaju takav course"));

        var act = async () => await controller.GetByCourse(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Lessoni nemaju takav course");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsListOfLessons_WhenLessonExists()
    {
        var dto = new LessonResponseDTO { Id = 1, Title = "Marketing", Content = "Odlican marketing imate!" };

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<LessonResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Title.Should().Be("Marketing");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenLessonsNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nema lessona sa tim id-om"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nema lessona sa tim id-om");

    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var dto = new LessonUpdateDTO { Title = "Marketing", Content = "Odlican marketing imate!" };
        var response =  new LessonResponseDTO { Id = 3, Title = "Marketing", Content = "Odlican marketing imate!" };
        
        mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<LessonResponseDTO>().Subject;
        returned.Title.Should().Be("Marketing");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenLessonNotFound()
    {
        var dto = new LessonUpdateDTO { Title = "Marketing", Content = "Odlican marketing imate!" };
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji lesson sa tim id-om"));

        var act = async () => await controller.Update(1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji lesson sa tim id-om");
    }
}