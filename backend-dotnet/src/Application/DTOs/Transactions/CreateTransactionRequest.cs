namespace Application.DTOs.Transactions;

public class CreateTransactionRequest
{
    public int CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

//Decisiones de diseño:

//Excluí Id porque se genera automáticamente en la BD (IDENTITY)
//Excluí CreatedAt porque se asigna automáticamente con DEFAULT GETDATE()
//Incluí solo los campos que el cliente debe proporcionar al crear una transacción
