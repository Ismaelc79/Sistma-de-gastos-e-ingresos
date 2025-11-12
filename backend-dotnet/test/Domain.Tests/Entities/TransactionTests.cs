using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class TransactionTests
{
    [Fact]
    public void Constructor_WithValidValues_ShouldInitialize()
    {
        var userId = Ulid.NewUlid();

        var transaction = new Transaction("Salary", 1000m, 1, userId);

        transaction.Name.Should().Be("Salary");
        transaction.Amount.Should().Be(1000m);
        transaction.CategoryId.Should().Be(1);
        transaction.UserId.Should().Be(userId);
        transaction.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_ShouldChangeProvidedFields()
    {
        var transaction = new Transaction("Initial", 50m, 1, Ulid.NewUlid(), "desc");

        transaction.Update(categoryId: 2, name: "Updated", description: "new", amount: 75m);

        transaction.Name.Should().Be("Updated");
        transaction.Description.Should().Be("new");
        transaction.CategoryId.Should().Be(2);
        transaction.Amount.Should().Be(75m);
    }

    [Theory]
    [InlineData("", 10)]
    [InlineData("Valid", 0)]
    public void Constructor_WithInvalidValues_ShouldThrow(string name, decimal amount)
    {
        Action act = () => new Transaction(name, amount, 1, Ulid.NewUlid());

        act.Should().Throw<ArgumentException>();
    }
}
