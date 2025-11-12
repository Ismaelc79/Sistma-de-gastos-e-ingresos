using Application.DTOs.Auth;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests;

public class UsersControllerTests
{
    [Fact]
    public async Task Get_ShouldReturnOk_WhenServiceReturnsUser()
    {
        var service = new Mock<IUserService>();
        var userId = Ulid.NewUlid();
        service.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(new UserDto { Name = "User" });
        var controller = new usersController(service.Object)
        {
            ControllerContext = ControllerTestHelper.WithUser(userId)
        };

        var result = await controller.Get();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Patch_WhenExceptionIsThrown_ShouldReturn401()
    {
        var service = new Mock<IUserService>();
        var userId = Ulid.NewUlid();
        service.Setup(s => s.UpdateUserProfileAsync(userId, It.IsAny<UpdateProfileRequest>()))
            .ThrowsAsync(new Exception("boom"));
        var controller = new usersController(service.Object)
        {
            ControllerContext = ControllerTestHelper.WithUser(userId)
        };

        var result = await controller.Patch(new UpdateProfileRequest());

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(401);
    }
}
