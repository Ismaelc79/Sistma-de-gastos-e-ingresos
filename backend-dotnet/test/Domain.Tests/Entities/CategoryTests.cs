using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class CategoryTests
{
    [Fact]
    public void Constructor_WithValidValues_ShouldSetProperties()
    {
        var userId = Ulid.NewUlid();

        var category = new Category("Food", "Expense", userId);

        category.Name.Should().Be("Food");
        category.Type.Should().Be("Expense");
        category.UserId.Should().Be(userId);
        category.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void EditCategory_ShouldUpdateValues_WhenProvided()
    {
        var category = new Category("Old", "Expense", Ulid.NewUlid());

        category.EditCategory("New", "Income");

        category.Name.Should().Be("New");
        category.Type.Should().Be("Income");
    }

    [Fact]
    public void Constructor_WithEmptyName_ShouldThrow()
    {
        Action act = () => new Category("", "Expense", Ulid.NewUlid());

        act.Should().Throw<ArgumentException>();
    }
}
