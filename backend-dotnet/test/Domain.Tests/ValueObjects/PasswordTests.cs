using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects;

public class PasswordTests
{
    [Fact]
    public void CreateHashed_ShouldHashPasswordAndVerifySuccesfully()
    {
        var password = Password.CreateHashed("StrongPass123!");

        password.Hash.Should().NotBeNullOrEmpty();
        password.Verify("StrongPass123!").Should().BeTrue();
    }

    [Fact]
    public void Verify_WithWrongPassword_ShouldReturnFalse()
    {
        var password = Password.CreateHashed("Correct#123");

        password.Verify("Wrong").Should().BeFalse();
    }

    [Fact]
    public void CreateHashed_WithEmptyPlainPassword_ShouldThrow()
    {
        Action act = () => Password.CreateHashed("");

        act.Should().Throw<ArgumentException>();
    }
}
