using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.DTOs.UserD;
using CourseManagementSystem.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class UserControllerTest
{
    private readonly Mock<IUserService> mockService;
    private readonly UserController controller;

    public UserControllerTest()
    {
        mockService = new Mock<IUserService>();
        controller = new UserController(mockService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnOk_WithListOfUsers()
    {
        var users = new List<UserResponseDTO>
        {
            new() { Id = 1, Username = "Josip", Email = "josip@gmail.com" },

            new() { Id = 2, Username = "Josip2", Email = "josip2@gmail.com" }
        };
        
        mockService.Setup(s=> s.GetAllAsync()).ReturnsAsync(users);
        
        var result = await controller.GetAll();
        
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<UserResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }
     
    [Fact]
    public async Task GetAllAsync_ReturnsOk_WithEmptyListOfUsers()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<UserResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<UserResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk_WhenUserExists()
    {
        var dto = new UserResponseDTO { Id = 1, Username = "Josip", Email = "josip@gmail.com" };

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<UserResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Username.Should().Be("Josip");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenUserNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nema usera sa tim id-om"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nema usera sa tim id-om");

    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var dto = new UserUpdateDTO { Username = "Josip", Email = "josip@gmail.com" };
        var response = new UserResponseDTO { Id = 1, Username = "Josip", Email = "josip@gmail.com" };

        mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

        var result = await controller.updateAsync(1, dto);
        
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<UserResponseDTO>().Subject;
        returned.Username.Should().Be("Josip");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenUserNotFound()
    {
        var dto = new UserUpdateDTO { Username = "Josip", Email = "josip@gmail.com" };
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji user sa tim id-om"));

        var act = async () => await controller.updateAsync(1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji user sa tim id-om");
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_WhenDeleted()
    {
        mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await controller.deleteAsync(1);

        result.Should().BeOfType<NoContentResult>();

    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenUserNotFound()
    {
        mockService.Setup(s => s.DeleteAsync(1))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji user kojeg trazite"));

        var act = async () => await controller.deleteAsync(1);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji user kojeg trazite");

    }
}