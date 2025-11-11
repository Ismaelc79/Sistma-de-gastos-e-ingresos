using Application.DTOs.Categories;
using Application.Services;
using Application.Tests.TestHelpers;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.Tests.Services;

public class CategoryServiceTests
{
    private static readonly AutoMapper.IMapper Mapper = MapperFactory.Create();

    [Fact]
    public async Task CreateCategoryAsync_ShouldPersistAndReturnDto()
    {
        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo.Setup(r => r.AddAsync(It.IsAny<Category>())).ReturnsAsync((Category c) => c);
        var unitOfWork = UnitOfWorkMockBuilder.Build(category: categoryRepo);

        var service = new CategoryService(unitOfWork.Object, Mapper);
        var request = new CreateCategoryRequest { Name = "Food", Type = "Expense", UserId = Ulid.NewUlid() };

        var result = await service.CreateCategoryAsync(request);

        categoryRepo.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == "Food" && c.Type == "Expense")), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Name.Should().Be("Food");
    }

    [Fact]
    public async Task GetCategoriesByTypeAsync_ShouldFilterByTypeIgnoringCase()
    {
        var userId = Ulid.NewUlid();
        var categories = new List<Category>
        {
            new("Salary", "Income", userId),
            new("Rent", "Expense", userId),
            new("Bonus", "income", userId)
        };

        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(categories);
        var unitOfWork = UnitOfWorkMockBuilder.Build(category: categoryRepo);

        var service = new CategoryService(unitOfWork.Object, Mapper);

        var result = await service.GetCategoriesByTypeAsync(userId, "INCOME");

        result.Should().HaveCount(2).And.AllSatisfy(c => c.Type.Should().BeOneOf("Income", "income"));
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryDoesNotExist_ShouldThrow()
    {
        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category?)null);
        var unitOfWork = UnitOfWorkMockBuilder.Build(category: categoryRepo);

        var service = new CategoryService(unitOfWork.Object, Mapper);

        var act = async () => await service.UpdateCategoryAsync(99, new CreateCategoryRequest());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenRepositoryReturnsTrue_ShouldPersistChanges()
    {
        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo.Setup(r => r.DeleteAsync(5)).ReturnsAsync(true);
        var unitOfWork = UnitOfWorkMockBuilder.Build(category: categoryRepo);

        var service = new CategoryService(unitOfWork.Object, Mapper);

        var result = await service.DeleteCategoryAsync(5);

        result.Should().BeTrue();
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
