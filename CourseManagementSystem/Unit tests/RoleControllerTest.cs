using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Role;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Services.Implementation;
using CourseManagementSystem.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class RoleControllerTest
{
    private readonly Mock<IRoleService> mockService;
    private readonly RoleController controller;

    public RoleControllerTest()
    {
        mockService = new Mock<IRoleService>();
        controller = new RoleController(mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithListOfRoles()
    {
        var roles = new List<RoleResponseDTO>
        {
            new() { Id = 1, Name = RoleE.Admin, UserRolesCount = 2},

            new() { Id = 2, Name = RoleE.User, UserRolesCount = 2 }

        };
        
        mockService.Setup(s=> s.GetAllAsync()).ReturnsAsync(roles);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<RoleResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAll_ReturnsOKWithEmptyList_WhenNoRoles()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<RoleResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<RoleResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenRoleExist()
    {
        var dto = new RoleResponseDTO { Id = 1, Name = RoleE.Admin, UserRolesCount = 2 };

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<RoleResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Name.Should().Be(RoleE.Admin);
    }

    [Fact]
    public async Task GetById_ThrowsKeyNotFoundException_WhenRoleNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nema role sa tim id-om"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nema role sa tim id-om");

    }

    [Fact]
    public async Task GetByName_ReturnsOk_WhenRoleExists()
    {
        var dto = new RoleResponseDTO { Id = 1, Name = RoleE.Admin, UserRolesCount = 2 };

        mockService.Setup(s => s.GetByNameAsync(RoleE.Admin)).ReturnsAsync(dto);
        
        var result = await controller.GetByName(RoleE.Admin);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<RoleResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Name.Should().Be(RoleE.Admin);
    }

    [Fact]
    public async Task GetByName_ThrowsKeyNotFoundException_WhenRoleNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nema role sa tim imenom"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nema role sa tim imenom");

    }
}