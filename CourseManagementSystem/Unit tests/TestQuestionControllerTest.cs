using CourseManagementSystem.Controllers;
using CourseManagementSystem.DTOs.Lesson;
using CourseManagementSystem.DTOs.TestQuestion;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Services.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseManagementSystem.Unit_tests;

public class TestQuestionControllerTest
{
    private readonly Mock<ITestQuestionService> mockService;
    private readonly TestQuestionController controller;

    public TestQuestionControllerTest()
    {
        mockService = new Mock<ITestQuestionService>();
        controller = new TestQuestionController(mockService.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnOk_WhenCreated()
    {
        var dto = new TestQuestionCreateDTO() { Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };
        var response =  new TestQuestionResponseDTO() { Id = 3, Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };
        
        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);
        
        var result = await controller.Create(dto);
        
        
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returned = createdResult.Value.Should().BeAssignableTo<TestQuestionResponseDTO>().Subject;
        returned.Question.Should().Be("Marketing");

    }

    [Fact]
    public async Task CreateAsync_TrowsKeyNotFoundException_WhenNotFound()
    {
        var dto = new TestQuestionCreateDTO() { Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };

        mockService.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji testQuestion"));

        var act = async () => await controller.Create(dto);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji testQuestion");

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
            .ThrowsAsync(new KeyNotFoundException("Nepostoji testQuestion koji trazite"));

        var act = async () => await controller.Delete(1);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji testQuestion koji trazite");

    }

    [Fact]
    public async Task GetByIdAsync_ReturnsListOfTestQuestions_WhenTestQuestionExists()
    {
        var dto = new TestQuestionResponseDTO() { Id= 1, Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };

        mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        
        var result = await controller.GetById(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeOfType<TestQuestionResponseDTO>().Subject;
        returned.Id.Should().Be(1);
        returned.Question.Should().Be("Marketing");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenTestQuestionNotFound()
    {
        mockService.Setup(s=> s.GetByIdAsync(100))
            .ThrowsAsync(new KeyNotFoundException("Nema TestQuestiona sa tim id-om"));

        var act = async () => await controller.GetById(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nema TestQuestiona sa tim id-om");

    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfTestQuestions_WhenTestQuestionExists()
    {
        var testQuestions = new List<TestQuestionResponseDTO>
        {
            new() { Id= 1, Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" },

            new() { Id= 2, Question = "Marketing2", CorrectAnswer = "Odlican marketing2 imate!" }

        };
        
        mockService.Setup(s=> s.GetAllAsync()).ReturnsAsync(testQuestions);
        
        var result = await controller.GetAll();
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<TestQuestionResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkWithEmptyList_WhenNoTestQuestionsExists()
    {
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<TestQuestionResponseDTO>());

        var result = await controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<TestQuestionResponseDTO>>().Subject;
        returned.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByTestAsync_ReturnsListOfTests_WhenExists()
    {
        var testQuestions = new List<TestQuestionResponseDTO>
        {
            new() { Id= 1, Question = "Marketing", CorrectAnswer = "Odlican marketing imate!", TestId = 1},

            new() { Id= 2, Question = "Marketing2", CorrectAnswer = "Odlican marketing2 imate!", TestId = 1}

        };

        mockService.Setup(s => s.GetByTestAsync(1))
            .ReturnsAsync(testQuestions);

        var result = await controller.GetByTest(1);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<IEnumerable<TestQuestionResponseDTO>>().Subject;
        returned.Should().HaveCount(2);
        returned.All(l => l.TestId == 1).Should().BeTrue();

    }

    [Fact]
    public async Task GetByTestAsync_ThrowsKeyNotFoundException_WhenTestQuestionNotFound()
    {
        mockService.Setup(s=> s.GetByTestAsync(100))
            .ThrowsAsync(new KeyNotFoundException("TestQuestion nema takav test"));

        var act = async () => await controller.GetByTest(100);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("TestQuestion nema takav test");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var dto = new TestQuestionUpdateDTO() { Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };
        var response =  new TestQuestionResponseDTO() { Id = 3, Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };

        mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

        var result = await controller.Update(1, dto);
        
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returned = okResult.Value.Should().BeAssignableTo<TestQuestionResponseDTO>().Subject;
        returned.Question.Should().Be("Marketing");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenTestQuestionNotFound()
    {
        var dto = new TestQuestionUpdateDTO() { Question = "Marketing", CorrectAnswer = "Odlican marketing imate!" };
        mockService.Setup(s => s.UpdateAsync(1, dto))
            .ThrowsAsync(new KeyNotFoundException("Nepostoji testQuestion sa tim id-om"));

        var act = async () => await controller.Update(1, dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Nepostoji testQuestion sa tim id-om");
    }
}