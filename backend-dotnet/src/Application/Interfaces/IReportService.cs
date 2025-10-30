using Application.DTOs.Reports;

namespace Application.Interfaces;

public interface IReportService
{
    Task<SummaryReportDto> GetSummaryReportAsync(string userId, DateTime startDate, DateTime endDate);
    Task<SummaryReportDto> GetSummaryReportByTypeAsync(string userId, string categoryType, DateTime startDate, DateTime endDate);
}
