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

        // Trae las ultimas transacciones desde hacec 6 meses (para recomendado)
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var last6MonthTransactions = await _unitOfWork.Transaction.GetByUserIdAndDateRangeAsync(userId, sixMonthsAgo, endDate);

        // Filtra las transacciones actuales que pertenecen a las categorías válidas
        var filteredTransactions = transactions
            .Where(t => categories.Any(c => c.Id == t.CategoryId))
            .ToList();

        // Agrupa las transacciones actuales por categoría
        var grouped = filteredTransactions
            .GroupBy(t => t.CategoryId)
            .Select(g =>
            {
                var category = categories.FirstOrDefault(c => c.Id == g.Key);
                var totalAmount = g.Sum(t => t.Amount);
                return new
                {
                    CategoryId = g.Key,
                    CategoryName = category?.Name ?? "Sin categoría",
                    CategoryType = category?.Type ?? "Desconocido",
                    TotalAmount = totalAmount
                };
            })
            .ToList();

        var total = grouped.Sum(x => x.TotalAmount);

        // Calcula los porcentajes recomendados dinámicos (solo para gastos)
        var expenseCategoryIds = categories
            .Where(c => string.Equals(c.Type, "Gasto", StringComparison.OrdinalIgnoreCase)
                     || string.Equals(c.Type, "Expense", StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Id)
            .ToHashSet();

        var monthlyGroups = last6MonthTransactions
            .Where(t => expenseCategoryIds.Contains(t.CategoryId))
            .GroupBy(t => new { Month = new DateTime(t.CreatedAt.Month, t.CreatedAt.Month, 1), t.CategoryId })
            .Select(g => new
            {
                g.Key.CategoryId,
                g.Key.Month,
                TotalAmount = g.Sum(x => x.Amount)
            })
            .ToList();

        // Total mensual de gastos
        var totalPerMonth = monthlyGroups
            .GroupBy(x => x.Month)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalAmount));

        // Promedio histórico de % mensual por categoría
        var recommendedByCategory = monthlyGroups
            .GroupBy(x => x.CategoryId)
            .Select(g =>
            {
                var avgPercent = g.Average(m =>
                    totalPerMonth.TryGetValue(m.Month, out var totalMonth) && totalMonth > 0
                        ? (m.TotalAmount / totalMonth) * 100m
                        : 0m);
                return new { CategoryId = g.Key, RecommendedPercent = Math.Round(avgPercent, 2) };
            })
            .ToDictionary(x => x.CategoryId, x => x.RecommendedPercent);

        // Construye el resumen final
        var summary = grouped.Select(x =>
        {
            var percent = total > 0 ? Math.Round((x.TotalAmount / total) * 100, 2) : 0;
            var recommended = 0m;

            // Solo aplicar recomendado si la categoría es de gasto
            if (expenseCategoryIds.Contains(x.CategoryId) &&
                recommendedByCategory.TryGetValue(x.CategoryId, out var rec))
            {
                recommended = rec;
            }

            return new CategorySummary
            {
                Name = x.CategoryName,
                TotalAmount = x.TotalAmount,
                Percentage = percent,
                PercentageRecommended = recommended
            };
        }).ToList();

        // Retorna el DTO de reporte
        return new SummaryReportDto
        {
            CategoryType = categoryType,
            TotalPercentage = summary.Sum(x => x.Percentage),
            TotalPercentageRecommended = summary.Sum(x => x.PercentageRecommended),
            TotalAmount = summary.Sum(x => x.TotalAmount),
            CategorySummaryList = summary
        };
    }
}
