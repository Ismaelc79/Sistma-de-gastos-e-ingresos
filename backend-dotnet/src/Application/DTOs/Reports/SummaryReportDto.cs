namespace Application.DTOs.Reports;

public class SummaryReportDto
{
    public string UserId { get; set; } = string.Empty;
    public int TotalTransactions { get; set; }
    public int IncomeCount { get; set; }
    public int ExpenseCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CategorySummary> CategoryBreakdown { get; set; } = new();
}

public class CategorySummary
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryType { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
}
