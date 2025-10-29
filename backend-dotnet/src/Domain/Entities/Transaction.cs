namespace Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Category? Category { get; set; }
    public User? User { get; set; }
}
