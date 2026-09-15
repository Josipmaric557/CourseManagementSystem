using System.Security.Claims;
using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Auth;
using CourseManagementSystem.Repository.Interfaces;
using CourseManagementSystem.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> mockService;
    private readonly AuthController controller;

    public AuthControllerTest()
    {
        mockService = new Mock<IAuthService>();
        controller = new AuthController(mockService.Object);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsOk_WhenUserIsRegistered()
    {
        var dto = new RegisterDTO
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "Test123!"
        };

        var response = new AuthResponseDTO
        {
            AccessToken = "fake_token",
            RefreshToken = "fake_refresh",
            UserId = 1,
            Username = "testuser",
            Roles = new List<string> { "User" }
        };

        mockService.Setup(s => s.RegisterAsync(dto))
            .ReturnsAsync(response);

        
        var result = await controller.Register(dto);

        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<AuthResponseDTO>().Subject;
        returned.Username.Should().Be("testuser");
        returned.AccessToken.Should().Be("fake_token");
    }

    [Fact]
    public async Task RegisterAsync_ThrowsInvalidOperationException_WhenUserEmailExists()
    {
        var dto = new RegisterDTO { Email = "existing@test.com", Password = "Test123!" };

        mockService.Setup(s => s.RegisterAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Korisnik s tim emailom već postoji."));
        
        var act = async () => await controller.Register(dto);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Korisnik s tim emailom već postoji.");
    }

    [Fact]
    public async Task LoginAsync_RetrunsOk_WhenValid()
    {
        var dto = new LoginDTO { Email = "test@test.com", Password = "Test123!" };

        var response = new AuthResponseDTO
        {
            AccessToken = "valid_token",
            RefreshToken = "valid_refresh",
            UserId = 1,
            Username = "testuser"
        };

        mockService.Setup(s => s.LoginAsync(dto))
            .ReturnsAsync(response);

        
        var result = await controller.Login(dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<AuthResponseDTO>().Subject;
        returned.AccessToken.Should().Be("valid_token");
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenWrongEmailOrPassword()
    {
        var dto = new LoginDTO { Email = "wrong@test.com", Password = "wrongpass" };

        mockService.Setup(s => s.LoginAsync(dto))
            .ThrowsAsync(new UnauthorizedAccessException("Pogrešan email ili lozinka."));

        
        var act = async () => await controller.Login(dto);
        
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Pogrešan email ili lozinka.");
    }

    [Fact]
    public async Task RefreshTokenAsync_ReturnsOk_WhenValid()
    {
        var dto = new RefreshTokenDTO { RefreshToken = "valid_refresh_token" };

        var response = new AuthResponseDTO
        {
            AccessToken = "new_access_token",
            RefreshToken = "new_refresh_token"
        };

        mockService.Setup(s => s.RefreshTokenAsync(dto.RefreshToken))
            .ReturnsAsync(response);

        
        var result = await controller.RefreshToken(dto);

        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<AuthResponseDTO>().Subject;
        returned.AccessToken.Should().Be("new_access_token");
    }

    [Fact]
    public async Task RefreshTokenAsync_ThrowsUnauthorizedAccessException_WhenInvalidToken()
    {
        var dto = new RefreshTokenDTO
        {
            RefreshToken = "invalid_refresh_token"
        };

        mockService.Setup(s => s.RefreshTokenAsync(dto.RefreshToken))
            .ThrowsAsync(new UnauthorizedAccessException("Nevažeći refresh token."));

        var act = async () => await controller.RefreshToken(dto);

        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Nevažeći refresh token.");
    }

    [Fact]
    public async Task LogoutAsync_ReturnsNoContent_WhenValidUser()
    {
        var userId = 1;
        
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claims
            }
        };

        mockService.Setup(s => s.LogoutAsync(userId))
            .Returns(Task.CompletedTask);

        var result = await controller.Logout();

        result.Should().BeOfType<NoContentResult>();

        mockService.Verify(s => s.LogoutAsync(userId), Times.Once);
    }

    [Fact]
    public async Task LogoutAsync_ThrowsUnauthorizedAccessException_WhenInvalidUser()
    {
        var userId = 1;
        
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claims
            }
        };

        mockService.Setup(s => s.LogoutAsync(userId))
            .ThrowsAsync(new KeyNotFoundException("Korisnik s ID-om 1 nije pronađen."));

        var act = async () => await controller.Logout();

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("Korisnik s ID-om 1 nije pronađen.");
    }
}