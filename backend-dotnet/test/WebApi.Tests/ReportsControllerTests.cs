using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests;

public class ReportsControllerTests
{
    [Fact]
    public async Task Get_WhenUserClaimMissing_ShouldReturnUnauthorized()
    {
        var controller = new reportsController(Mock.Of<IReportService>())
        {
            ControllerContext = ControllerTestHelper.WithUser(null)
        };

        var response = await controller.Get("Expense", DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);

        response.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Get_WhenUserClaimPresent_ShouldReturnOk()
    {
        var service = new Mock<IReportService>();
        service.Setup(s => s.GetCategorySummaryAsync(It.IsAny<Ulid>(), "Expense", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Application.DTOs.Reports.SummaryReportDto());
        var controller = new reportsController(service.Object)
        {
            ControllerContext = ControllerTestHelper.WithUser(Ulid.NewUlid())
        };

        var result = await controller.Get("Expense", DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);

        result.Should().BeOfType<OkObjectResult>();
    }
}
