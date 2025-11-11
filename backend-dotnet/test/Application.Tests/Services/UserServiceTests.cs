using Application.DTOs.Auth;
using Application.Services;
using Application.Tests.TestHelpers;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.Tests.Services;

public class UserServiceTests
{
    private static readonly AutoMapper.IMapper Mapper = MapperFactory.Create();

    [Fact]
    public async Task UpdateUserProfileAsync_ShouldUpdatePhoneCurrencyAndPassword()
    {
        var user = BuildUser();
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        userRepo.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
        var uow = UnitOfWorkMockBuilder.Build(user: userRepo);
        var service = new UserService(uow.Object, Mapper);

        var request = new UpdateProfileRequest
        {
            Name = "Updated",
            Phone = "+18095551234",
            Currency = "USD",
            Language = "es",
            Avatar = "avatar.png",
            Password = "Passw0rd!",
            NewPassword = "NewPassw0rd!"
        };

        var result = await service.UpdateUserProfileAsync(user.Id, request);

        result.Name.Should().Be("Updated");
        user.Phone!.Value.Should().Be("+18095551234");
        user.Currency.Code.Should().Be("USD");
        user.PasswordHash.Verify("NewPassw0rd!").Should().BeTrue();
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WhenCurrentPasswordIsWrong_ShouldThrow()
    {
        var user = BuildUser();
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        var uow = UnitOfWorkMockBuilder.Build(user: userRepo);
        var service = new UserService(uow.Object, Mapper);

        var request = new UpdateProfileRequest
        {
            Password = "Wrong",
            NewPassword = "Something123"
        };

        Func<Task> act = async () => await service.UpdateUserProfileAsync(user.Id, request);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenCurrentPasswordDoesNotMatch_ShouldThrow()
    {
        var user = BuildUser();
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        var uow = UnitOfWorkMockBuilder.Build(user: userRepo);
        var service = new UserService(uow.Object, Mapper);

        Func<Task> act = async () => await service.ChangePasswordAsync(user.Id, "invalid", "NewPassw0rd!");

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    private static User BuildUser() =>
        new User(
            id: Ulid.NewUlid(),
            name: "Test",
            email: new Email("user@example.com"),
            password: Password.CreateHashed("Passw0rd!")
        );
}
