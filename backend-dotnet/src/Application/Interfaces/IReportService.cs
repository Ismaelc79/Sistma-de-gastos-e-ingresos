using Application.DTOs.Reports;

namespace Application.Interfaces;

public interface IReportService
{
    Task<SummaryReportDto> GetCategorySummaryAsync(Ulid userId, string categoryType, DateTime startDate, DateTime endDate);
}
