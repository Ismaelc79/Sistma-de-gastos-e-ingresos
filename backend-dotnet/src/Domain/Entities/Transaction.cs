namespace Domain.Entities
{
    public class Transaction
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public decimal Amount { get; private set; } // Podemos agregar

        public Transaction(string name, decimal amount, int categoryId, string userId, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre obligatorio");
            if (amount <= 0) throw new ArgumentException("Monto debe ser mayor que cero");

            Name = name;
            Amount = amount;
            CategoryId = categoryId;
            UserId = userId;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
