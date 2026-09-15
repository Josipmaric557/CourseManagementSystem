using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.DTOs.Test;
using CourseManagementSystem.Services.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class TestControllerTest
{
    private readonly Mock<ITestService> mockService;
    private readonly TestController controller;

    public TestControllerTest()
    {
        mockService = new Mock<ITestService>();
        controller = new TestController(mockService.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnsOk_WhenCreated()
    {
        var dto = new TestCreateDTO { Title = "Marketing", PassingScore = 50};
        var response =  new TestResponseDTO { Id = 3, Title = "Marketing", PassingScore = 50};
        
        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);
        
        var result = await controller.Create(dto);
        
        
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<TestResponseDTO>().Subject;
        returned.Title.Should().Be("Marketing");
    }

    [Fact]
    public async Task CreateAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        var dto = new TestCreateDTO { Title = "Marketing", PassingScore = 50};

        mockService.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji test"));

        var act = async () => await controller.Create(dto);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji test");

    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();

    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji test koji trazite"));

        var act = async () => await controller.Delete(1);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji test koji trazite");

    }

    [Fact]
    public async Task GetAllAsync_ReturnsOk_WithListOfTests()
    {
        var tests = new List<TestResponseDTO>
        {
            new() {Id = 1, Title = "Marketing", PassingScore = 50},

            new() {Id = 2, Title = "Marketing", PassingScore = 50}

        };
        
        mockService.Setup(s=> s.GetAllAsync()).ReturnsAsync(tests);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<TestResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithEmptyListOfTests_WhenNoTests()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<TestResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<TestResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk_WhenExists()
    {
        var dto = new TestResponseDTO { Id = 1, Title = "Marketing", PassingScore = 50};

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<TestResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Title.Should().Be("Marketing");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundExcpetion_WhenNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nema testa sa tim id-om"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nema testa sa tim id-om");

    }

    [Fact]
    public async Task GetByLessonAsync_ReturnsListOfTests_WhenExists()
    {
        var tests = new List<TestResponseDTO>
        {
            new() {Id = 1, Title = "Marketing", PassingScore = 50, LessonId = 1},

            new() {Id = 2, Title = "Marketing", PassingScore = 50, LessonId = 1}

        };
        
        mockService.Setup(s => s.GetByLessonAsync(1))
            .ReturnsAsync(tests);

        var result = await controller.GetByLesson(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<TestResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(l => l.LessonId == 1).Should().BeTrue();

    }

    [Fact]
    public async Task GetByLessonAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        mockService.Setup(s=> s.GetByLessonAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Test nema takav lesson"));

        var act = async () => await controller.GetByLesson(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Test nema takav lesson");
    }

    [Fact]
    public async Task UpdateAsync_RetunsOk_WhenUpdated()
    {
        var dto = new TestUpdateDTO { Title = "Marketing"};
        var response =  new TestResponseDTO { Id = 3, Title = "Marketing", PassingScore = 50};

        mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<TestResponseDTO>().Subject;
        returned.Title.Should().Be("Marketing");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        var dto = new TestUpdateDTO { Title = "Marketing" };
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji test sa tim id-om"));

        var act = async () => await controller.Update(1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji test sa tim id-om");
    }
}