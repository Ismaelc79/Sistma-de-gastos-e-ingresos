using Application.Services;
using Application.Tests.TestHelpers;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.Tests.Services;

public class ReportServiceTests
{
    private static readonly AutoMapper.IMapper Mapper = MapperFactory.Create();

    [Fact]
    public async Task GetCategorySummaryAsync_ShouldAggregateTotalsAndPercentages()
    {
        var userId = Ulid.NewUlid();
        var categories = new List<Category> { new("Food", "Expense", userId) { } };
        var now = DateTime.UtcNow;
        var currentTransactions = new List<Transaction>
        {
            BuildTransaction("Groceries", 100m, categories[0].Id, userId, now),
            BuildTransaction("Dining", 200m, categories[0].Id, userId, now)
        };
        var historyTransactions = new List<Transaction>
        {
            BuildTransaction("History1", 150m, categories[0].Id, userId, now.AddMonths(-1)),
            BuildTransaction("History2", 50m, categories[0].Id, userId, now.AddMonths(-2))
        };

        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo.Setup(r => r.GetByUserIdAndTypeAsync(userId, "Expense")).ReturnsAsync(categories);

        var transactionRepo = new Mock<ITransactionRepository>();
        transactionRepo.SetupSequence(r => r.GetByUserIdAndDateRangeAsync(userId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(currentTransactions)
            .ReturnsAsync(historyTransactions);

        var uow = UnitOfWorkMockBuilder.Build(category: categoryRepo, transaction: transactionRepo);
        var service = new ReportService(uow.Object, Mapper);

        var summary = await service.GetCategorySummaryAsync(userId, "Expense", now.AddMonths(-1), now);

        summary.TotalAmount.Should().Be(300m);
        summary.CategorySummaryList.Should().ContainSingle();
        summary.CategorySummaryList.First().Percentage.Should().Be(100m);
    }

    [Fact]
    public async Task GetCategorySummaryAsync_WhenNoTransactions_ShouldReturnZeroTotals()
    {
        var userId = Ulid.NewUlid();
        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo.Setup(r => r.GetByUserIdAndTypeAsync(userId, "Expense")).ReturnsAsync(new List<Category>());

        var transactionRepo = new Mock<ITransactionRepository>();
        transactionRepo.Setup(r => r.GetByUserIdAndDateRangeAsync(userId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Transaction>());

        var uow = UnitOfWorkMockBuilder.Build(category: categoryRepo, transaction: transactionRepo);
        var service = new ReportService(uow.Object, Mapper);

        var summary = await service.GetCategorySummaryAsync(userId, "Expense", DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);

        summary.TotalAmount.Should().Be(0);
        summary.CategorySummaryList.Should().BeEmpty();
    }

    private static Transaction BuildTransaction(string name, decimal amount, int categoryId, Ulid userId, DateTime createdAt)
    {
        var transaction = new Transaction(name, amount, categoryId, userId);
        typeof(Transaction).GetProperty(nameof(Transaction.CreatedAt))!
            .SetValue(transaction, createdAt);
        return transaction;
    }
}
