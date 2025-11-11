using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+1 809-555-1234", "+18095551234")]
    [InlineData("8095550000", "8095550000")]
    public void Constructor_WithValidFormats_ShouldNormalizeAndStore(string input, string expected)
    {
        var phoneNumber = new PhoneNumber(input);

        phoneNumber.Value.Should().Be(expected);
        phoneNumber.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc123")]
    [InlineData("123")]
    public void Constructor_WithInvalidFormat_ShouldThrow(string input)
    {
        Action act = () => new PhoneNumber(input);

        act.Should().Throw<ArgumentException>();
    }
}
