using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Course;
using CourseManagementSystem.Services.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class CourseControllerTest
{
    private readonly Mock<ICourseService> mockService;
    private readonly CourseController controller;

    public CourseControllerTest()
    {
        mockService = new Mock<ICourseService>();
        controller = new CourseController(mockService.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnsOk_WhenCreated()
    {
        var dto = new CourseCreateDTO {Title = "Title", Description = "Description", Price = 53};
        var response = new CourseResponseDTO {Id = 1,  Title = "Title", Description = "Description", Price = 53};

        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);
        
        var result = await controller.CreateAsync(dto);

        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<CourseResponseDTO>().Subject;
        returned.Title.Should().Be("Title");
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenCreateFails()
    {
        var dto = new CourseCreateDTO {Title = "Title", Description = "Description", Price = 53};

        mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("Kategorija sa tim id ne postoji"));

        var act = async () => await controller.CreateAsync(dto);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Kategorija sa tim id ne postoji");
    }

    [Fact]
    public async Task DeleteAsync_ReturnNoContent_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1));

        var result = await controller.DeleteAsync(1);
        
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenCourseNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new Exception("Course sa tim id ne postoji"));

        var act = async () => await controller.DeleteAsync(1);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Course sa tim id ne postoji");
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenTryDeleteCourseThatHaveUsers()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new Exception("nemozes obrisati tecaj koji ima korisnike!"));

        var act = async () => await controller.DeleteAsync(1);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("nemozes obrisati tecaj koji ima korisnike!");
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithListOfCourses()
    {
        var courses = new List<CourseResponseDTO>
        {
            new() { Id = 1, Title = "Title", Description = "Description", Price = 53 },
            new() { Id = 2, Title = "Title", Description = "Description", Price = 53 },
        };

        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(courses);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<CourseResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithEmptyList_WhenNoCategories()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CourseResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<CourseResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByCategoryAsync_ReturnsListOfCourses_WhenCategoryExists()
    {
        var courses = new List<CourseResponseDTO>
        {
            new() { Id = 1, Title = "C# osnove", CategoryId = 1 },
            new() { Id = 2, Title = "ASP.NET Core", CategoryId = 1 }
        };

        mockService.Setup(s => s.GetByCategoryAsync(1))
            .ReturnsAsync(courses);

        var result = await controller.GetByCategory(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<CourseResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(c => c.CategoryId == 1).Should().BeTrue();
    }

    [Fact]
    public async Task GetByCategoryAsync_RetunsOk_WithEmptyList_WhenNoCourses()
    {
        mockService.Setup(s => s.GetByCategoryAsync(1))
            .ReturnsAsync(new List<CourseResponseDTO>());

        var result = await controller.GetByCategory(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<CourseResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    
    }

    [Fact]
    public async Task GetByCategoryAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
    {
        mockService.Setup(s=> s.GetByCategoryAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Kategorija sa id nije pronadena"));

        var act = async () => await controller.GetByCategory(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("kategorija sa id nije pronadena");
    
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenIdExists()
    {
        var course = new CourseResponseDTO
            { Id = 1, Title = "C# osnove", CategoryId = 1 };
            
        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(course);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<CourseResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Title.Should().Be("C# osnove");
    }

    [Fact]
    public async Task GetById_ThrowsKeyNotFoundException_WhenIdNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("tecaj sa tim id nije pronaden"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("tecaj sa tim id nije pronaden");

    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var dto = new CourseUpdateDTO {Title = "Title", Description = "Description", Price = 53};
        var response = new CourseResponseDTO {Id = 1,  Title = "Title", Description = "Description", Price = 53};

        mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<CourseResponseDTO>().Subject;
        returned.Title.Should().Be("Title");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenCourseNotFound()
    {
        var dto = new CourseUpdateDTO {Title = "Title", Description = "Description", Price = 53};
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji course sa tim id-om"));

        var act = async () => await controller.Update(1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji course sa tim id-om");
    }
    
    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
    {
        var dto = new CourseUpdateDTO { CategoryId = 999 };

        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Kategorija s ID-om 999 nije pronađena."));

        
        var act = async () => await controller.Update(1, dto);

        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Kategorija s ID-om 999 nije pronađena.");
    }

}