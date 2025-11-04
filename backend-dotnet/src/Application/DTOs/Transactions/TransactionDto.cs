namespace Application.DTOs.Transactions;

public class TransactionDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; } = int.MinValue;
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }
}
    