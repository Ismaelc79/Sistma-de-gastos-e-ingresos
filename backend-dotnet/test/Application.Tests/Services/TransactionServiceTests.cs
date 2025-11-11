using Application.DTOs.Transactions;
using Application.Services;
using Application.Tests.TestHelpers;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.Tests.Services;

public class TransactionServiceTests
{
    private static readonly AutoMapper.IMapper Mapper = MapperFactory.Create();

    [Fact]
    public async Task CreateTransactionAsync_ShouldPersistAndReturnDto()
    {
        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction t) => t);
        var uow = UnitOfWorkMockBuilder.Build(transaction: repo);
        var service = new TransactionService(uow.Object, Mapper);
        var request = new CreateTransactionRequest
        {
            Name = "Payment",
            Amount = 150,
            CategoryId = 1,
            UserId = Ulid.NewUlid(),
            Description = "Rent"
        };

        var result = await service.CreateTransactionAsync(request);

        repo.Verify(r => r.AddAsync(It.Is<Transaction>(t => t.Amount == 150)), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Amount.Should().Be(150);
    }

    [Fact]
    public async Task UpdateTransactionAsync_WhenNotFound_ShouldThrow()
    {
        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Transaction?)null);
        var uow = UnitOfWorkMockBuilder.Build(transaction: repo);
        var service = new TransactionService(uow.Object, Mapper);

        Func<Task> act = async () => await service.UpdateTransactionAsync(1, new CreateTransactionRequest());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetTransactionsByUserIdAndDateRangeAsync_ShouldReturnMappedDtos()
    {
        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByUserIdAndDateRangeAsync(It.IsAny<Ulid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Transaction>
            {
                new("Salary", 1000, 1, Ulid.NewUlid()),
                new("Groceries", 100, 2, Ulid.NewUlid())
            });
        var uow = UnitOfWorkMockBuilder.Build(transaction: repo);
        var service = new TransactionService(uow.Object, Mapper);

        var result = await service.GetTransactionsByUserIdAndDateRangeAsync(Ulid.NewUlid(), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteTransactionAsync_ShouldReturnRepositoryResult()
    {
        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);
        var uow = UnitOfWorkMockBuilder.Build(transaction: repo);
        var service = new TransactionService(uow.Object, Mapper);

        var result = await service.DeleteTransactionAsync(7);

        result.Should().BeTrue();
    }
}
