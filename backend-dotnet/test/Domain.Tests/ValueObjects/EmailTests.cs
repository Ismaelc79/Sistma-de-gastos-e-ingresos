using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Constructor_WithValidValue_ShouldStoreNormalizedValue()
    {
        // Act
        var email = new Email("user@example.com");

        // Assert
        email.Value.Should().Be("user@example.com");
        email.ToString().Should().Be("user@example.com");
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("   ")]
    public void Constructor_WithInvalidValue_ShouldThrow(string value)
    {
        // Act
        var action = () => new Email(value);

        // Assert
        action.Should().Throw<ArgumentException>();
    }
}
