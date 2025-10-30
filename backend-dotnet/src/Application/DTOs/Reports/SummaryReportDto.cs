namespace Application.DTOs.Reports;

public class SummaryReportDto
{
    public string CategoryType { get; set; } = string.Empty;
    public int TotalPercentage{ get; set; }
    public int TotalPercentageRecommended { get; set; }
    public List<CategorySummary>? CategorySummaryList { get; set; } = new();
}

public class CategorySummary
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Percentage { get; set; }
    public decimal PercentageRecommended { get; set; }

}
