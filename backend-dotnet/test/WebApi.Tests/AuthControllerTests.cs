using Application.DTOs.Auth;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_ShouldReturnOkWhenServiceSucceeds()
    {
        var service = new Mock<IAuthService>();
        service.Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>())).ReturnsAsync(new AuthResponse());
        var controller = new authController(service.Object);

        var result = await controller.Register(new RegisterRequest());

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Login_WhenServiceThrows_ShouldReturn401()
    {
        var service = new Mock<IAuthService>();
        service.Setup(s => s.LoginAsync(It.IsAny<LoginRequest>())).ThrowsAsync(new Exception("boom"));
        var controller = new authController(service.Object);

        var result = await controller.login(new LoginRequest());

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(401);
    }
}
