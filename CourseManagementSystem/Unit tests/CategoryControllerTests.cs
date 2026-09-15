using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Category;
using CourseManagementSystem.Services.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> mockService;
    private readonly CategoryController controller;

    public CategoryControllerTests()
    {
        mockService = new Mock<ICategoryService>();
        controller = new CategoryController(mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_withListOfCategories()
    {
        var categories = new List<CategoryResponseDTO>
        {
            new() { Id = 1, Name = "Programiranje", Description = "Category 1", CoursesCount = 3 },

            new() { Id = 1, Name = "Dizajn", Description = "Category 2", CoursesCount = 5 },

        };
        
        mockService.Setup(s=> s.GetAllAsync()).ReturnsAsync(categories);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<CategoryResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetAll_ReturnsOkWhenEmptyList_WhenNoCategories()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CategoryResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<CategoryResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WithCategoryExists()
    {
        var category = new CategoryResponseDTO
            { Id = 1, Name = "Programiranje", Description = "Category 1", CoursesCount = 3 };

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<CategoryResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Name.Should().Be("Programiranje");
    }
    
    [Fact]
    public async Task GetById_ThrowsKeyNotFountException_WhenCategoryNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji kategorija sa id 100"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji kategorija sa id 100");
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenCategoryCreated()
    {
        var dto = new CategoryCreateDTO { Name = "Marketing", Description = "Odlican marketing imate!" };
        var response =  new CategoryResponseDTO { Id = 3, Name = "marketing" };
        
        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);
        
        var result = await controller.Create(dto);
        
        
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<CategoryResponseDTO>().Subject;
        returned.Name.Should().Be("marketing");
    }

    [Fact]
    public async Task CreateAsync_ThrowsInvalidOperationException_whennameExistAlready()
    {
        var dto = new CategoryCreateDTO { Name = "Programiranje" };
        
        mockService.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Vec postoji ovo ime"));

        var act = async () => await controller.Create(dto);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Vec postoji ovo ime");
    }


    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var dto  = new CategoryUpdateDTO { Name = "Novo ime" };
        var response = new CategoryResponseDTO { Id = 1, Name = "Novo ime" };

        mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<CategoryResponseDTO>().Subject;
        returned.Name.Should().Be("Novo ime");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
    {
        var dto = new CategoryUpdateDTO { Name = "Programiranje" };
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji kategorija sa id"));

        var act = async () => await controller.Update(1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji kategorija sa id");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenCategoryNameExistsAlready()
    {
        var dto = new CategoryUpdateDTO { Name = "Programiranje" };
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new Exception("Vec postoji ovo ime"));

        var act = async () => await controller.Update(1, dto);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Vec postoji ovo ime");
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji kategorija sa id 100"));

        var act = async () => await controller.Delete(1);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji kategorija sa id 100");
    }

    [Fact]
    public async Task DeleteAsync_ThorwsException_WhenCategoryHaveCourses()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new Exception("nemozes brisat kategoriju koja ima aktivne tecajeve"));

        var act = async () => await controller.Delete(1);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("nemozes brisat kategoriju koja ima aktivne tecajeve");
    }
}