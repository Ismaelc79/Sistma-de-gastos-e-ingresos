using Application.DTOs.Reports;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SummaryReportDto> GetSummaryReportAsync(Ulid userId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _unitOfWork.Transaction.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);

        var report = new SummaryReportDto
        {
            
        };

        // Agrupar por categoría
        var groupedByCategory = transactions.GroupBy(t => t.CategoryId);

        foreach (var group in groupedByCategory)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(group.Key);

            if (category != null)
            {
                var categorySummary = new CategorySummary
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    CategoryType = category.Type,
                    TransactionCount = group.Count()
                };

                report.CategoryBreakdown.Add(categorySummary);

                // Contar por tipo
                if (category.Type.Equals("Income", StringComparison.OrdinalIgnoreCase))
                {
                    report.IncomeCount += group.Count();
                }
                else if (category.Type.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    report.ExpenseCount += group.Count();
                }
            }
        }

        return report;
    }

    public async Task<SummaryReportDto> GetSummaryReportByTypeAsync(string userId, string categoryType, DateTime startDate, DateTime endDate)
    {
        var transactions = await _unitOfWork.Transactions.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);

        // Filtrar por tipo de categoría
        var filteredTransactions = new List<Domain.Entities.Transaction>();
        foreach (var transaction in transactions)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(transaction.CategoryId);
            if (category != null && category.Type.Equals(categoryType, StringComparison.OrdinalIgnoreCase))
            {
                filteredTransactions.Add(transaction);
            }
        }

        var report = new SummaryReportDto
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate,
            TotalTransactions = filteredTransactions.Count(),
            IncomeCount = categoryType.Equals("Income", StringComparison.OrdinalIgnoreCase) ? filteredTransactions.Count() : 0,
            ExpenseCount = categoryType.Equals("Expense", StringComparison.OrdinalIgnoreCase) ? filteredTransactions.Count() : 0,
            CategoryBreakdown = new List<CategorySummary>()
        };

        // Agrupar por categoría
        var groupedByCategory = filteredTransactions.GroupBy(t => t.CategoryId);

        foreach (var group in groupedByCategory)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(group.Key);
            if (category != null)
            {
                var categorySummary = new CategorySummary
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    CategoryType = category.Type,
                    TransactionCount = group.Count()
                };

                report.CategoryBreakdown.Add(categorySummary);
            }
        }

        return report;
    }
}
