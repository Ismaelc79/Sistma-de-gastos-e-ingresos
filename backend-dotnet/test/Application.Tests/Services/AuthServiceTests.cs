using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Services;
using Application.Tests.TestHelpers;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Security.Claims;

namespace Application.Tests.Services;

public class AuthServiceTests
{
    private static readonly AutoMapper.IMapper Mapper = MapperFactory.Create();

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldThrow()
    {
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.EmailExistsAsync("taken@example.com")).ReturnsAsync(true);
        var uow = UnitOfWorkMockBuilder.Build(user: userRepo);

        var service = new AuthService(uow.Object, Mock.Of<ITokenService>(), Mapper);
        var request = new RegisterRequest { Email = "taken@example.com", Name = "User", Password = "Pass123!" };

        Func<Task> act = async () => await service.RegisterAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrow()
    {
        var user = BuildUser();
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByEmailAsync(user.Email.Value)).ReturnsAsync(user);
        var refreshRepo = new Mock<IRefreshTokenRepository>();
        refreshRepo.Setup(r => r.GetByUserIdAsync(user.Id)).ReturnsAsync(new List<RefreshToken>());
        var uow = UnitOfWorkMockBuilder.Build(user: userRepo, refreshToken: refreshRepo);
        var service = new AuthService(uow.Object, Mock.Of<ITokenService>(), Mapper);

        Func<Task> act = async () => await service.LoginAsync(new LoginRequest { Email = user.Email.Value, Password = "Wrong!" });

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task RefreshTokenAsync_WithValidTokens_ShouldReturnNewTokens()
    {
        var user = BuildUser();
        var refreshToken = new RefreshToken(Ulid.NewUlid(), user.Id, "jwt-old", DateTime.UtcNow.AddDays(1));

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var refreshRepo = new Mock<IRefreshTokenRepository>();
        refreshRepo.Setup(r => r.GetByJwtIdAsync("jwt-old")).ReturnsAsync(refreshToken);
        refreshRepo.Setup(r => r.GetByUserIdAsync(user.Id)).ReturnsAsync(new List<RefreshToken> { refreshToken });

        var tokenService = new Mock<ITokenService>();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }));
        tokenService.Setup(t => t.GetPrincipalFromExpiredToken("expired-access")).Returns(principal);
        tokenService.Setup(t => t.GetJwtIdFromToken("expired-access")).Returns("jwt-old");
        tokenService.Setup(t => t.GenerateAccessToken(user)).Returns("new-access");
        tokenService.Setup(t => t.GetJwtIdFromToken("new-access")).Returns("jwt-new");
        tokenService.Setup(t => t.GenerateRefreshToken()).Returns("new-refresh");

        var uow = UnitOfWorkMockBuilder.Build(user: userRepo, refreshToken: refreshRepo);
        var service = new AuthService(uow.Object, tokenService.Object, Mapper);

        var response = await service.RefreshTokenAsync(new RefreshTokenRequest
        {
            AccessToken = "expired-access",
            RefreshToken = "old-refresh"
        });

        response.AccessToken.Should().Be("new-access");
        response.RefreshToken.Should().Be("new-refresh");
        tokenService.Verify(t => t.GenerateAccessToken(user), Times.Once);
        refreshRepo.Verify(r => r.RevokeTokenAsync(refreshToken.Id), Times.Once);
        refreshRepo.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
    }

    private static User BuildUser() =>
        new User(
            id: Ulid.NewUlid(),
            name: "Test",
            email: new Email("user@example.com"),
            password: Password.CreateHashed("Pass123!")
        );
}
