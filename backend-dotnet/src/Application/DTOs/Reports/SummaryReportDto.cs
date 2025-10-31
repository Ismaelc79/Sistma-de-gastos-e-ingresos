namespace Application.DTOs.Reports;

public class SummaryReportDto
{
    public string CategoryType { get; set; } = string.Empty;
    public decimal TotalPercentage{ get; set; }
    public decimal TotalPercentageRecommended { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CategorySummary> CategorySummaryList { get; set; } = new();
}
