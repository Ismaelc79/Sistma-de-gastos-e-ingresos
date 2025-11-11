using Application.DTOs.Categories;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests;

public class CategoriesControllerTests
{
    [Fact]
    public async Task GetCategoriesByUserId_ShouldReturnOkResultWithPayload()
    {
        var service = new Mock<ICategoryService>();
        var userId = Ulid.NewUlid();
        service.Setup(s => s.GetCategoriesByUserIdAsync(userId)).ReturnsAsync(new List<CategoryDto> { new() { Name = "Food" } });

        var controller = new categoriesController(service.Object)
        {
            ControllerContext = ControllerTestHelper.WithUser(userId)
        };

        var result = await controller.GetCategoriesByUserId();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<CategoryDto>>();
    }

    [Fact]
    public async Task Add_WhenUserClaimMissing_ShouldReturnUnauthorized()
    {
        var controller = new categoriesController(Mock.Of<ICategoryService>())
        {
            ControllerContext = ControllerTestHelper.WithUser(null)
        };

        var result = await controller.Add(new CreateCategoryRequest());

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }
}
