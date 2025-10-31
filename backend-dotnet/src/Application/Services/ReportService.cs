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

    public async Task<SummaryReportDto> GetCategorySummaryAsync(Ulid userId, string categoryType, DateTime startDate, DateTime endDate)
    {
        // Tra las categorias filtradas por tipo
        var categories = await _unitOfWork.Category.GetByUserIdAndTypeAsync(userId, categoryType);

        // Trae las transacciones filtradas por fecha
        var transactions = await _unitOfWork.Transaction.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);

        // Filtra por categorias

        var filteredTransactions = transactions.Where(x => transactions.Any(a => a.Id.Equals(x.CategoryId))).ToList();

        // Agrupa por categoria
        var grouped = filteredTransactions
            .GroupBy(t => t.CategoryId)
            .Select(g =>
            {
                var category = categories.FirstOrDefault(c => c.Id == g.Key);
                var totalAmount = g.Sum(t => t.Amount);
                return new
                {
                    CategoryName = category?.Name ?? "Sin categoría",
                    TotalAmount = totalAmount,
                    CategoryType = category?.Type
                };
            })
            .ToList();

        var total = grouped.Sum(x => x.TotalAmount);

        // Calcular % recomendado
        decimal GetRecommendedPercent(string? type) => type switch
        {
            "Gasto" or "Expense" => 30m,
            "Ingreso" or "Income" => 0m,
            _ => 0m
        };

        var summary = grouped.Select(x => new CategorySummary
        {
            Name = x.CategoryName,
            TotalAmount = x.TotalAmount,
            Percentage = total > 0 ? Math.Round((x.TotalAmount / total) * 100, 2) : 0,
            PercentageRecommended = GetRecommendedPercent(x.CategoryType)
        }).ToList();

        return new SummaryReportDto
        {
            CategoryType = categoryType,
            TotalPercentage = summary.Sum(x=>x.Percentage),
            TotalPercentageRecommended = summary.Sum(x=>x.PercentageRecommended),
            TotalAmount = summary.Sum(x=>x.TotalAmount)
        };
    }
}
