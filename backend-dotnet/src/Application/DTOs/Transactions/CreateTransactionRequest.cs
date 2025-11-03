namespace Application.DTOs.Transactions;

public class CreateTransactionRequest
{
    public int CategoryId { get; set; }
    public Ulid UserId { get; set; } = Ulid.Empty!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

//Decisiones de diseño:

//Excluí Id porque se genera automáticamente en la BD (IDENTITY)
//Excluí CreatedAt porque se asigna automáticamente con DEFAULT GETDATE()
//Incluí solo los campos que el cliente debe proporcionar al crear una transacción
