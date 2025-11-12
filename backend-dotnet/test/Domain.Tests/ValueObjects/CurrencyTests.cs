using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects;

public class CurrencyTests
{
    [Fact]
    public void Constructor_WithLowercaseCode_ShouldNormalizeToUppercase()
    {
        var currency = new Currency("dop");

        currency.Code.Should().Be("DOP");
        currency.ToString().Should().Be("DOP");
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("USDX")]
    public void Constructor_WithInvalidCode_ShouldThrow(string code)
    {
        Action act = () => new Currency(code);

        act.Should().Throw<ArgumentException>();
    }
}
