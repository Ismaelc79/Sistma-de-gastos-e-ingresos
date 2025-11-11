using Application.DTOs.Transactions;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests;

public class TransactionsControllerTests
{
    [Fact]
    public async Task Get_ShouldReturnUnauthorizedWhenClaimMissing()
    {
        var controller = new transactionsController(Mock.Of<ITransactionService>())
        {
            ControllerContext = ControllerTestHelper.WithUser(null)
        };

        var response = await controller.Get();

        response.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Add_ShouldReturnCreatedWhenServiceSucceeds()
    {
        var service = new Mock<ITransactionService>();
        service.Setup(s => s.CreateTransactionAsync(It.IsAny<CreateTransactionRequest>()))
            .ReturnsAsync(new TransactionDto { Name = "Test" });
        var userId = Ulid.NewUlid();
        var controller = new transactionsController(service.Object)
        {
            ControllerContext = ControllerTestHelper.WithUser(userId)
        };

        var result = await controller.Add(new CreateTransactionRequest { Name = "Test" });

        result.Should().BeOfType<CreatedResult>();
        service.Verify(s => s.CreateTransactionAsync(It.Is<CreateTransactionRequest>(r => r.UserId == userId)), Times.Once);
    }
}
