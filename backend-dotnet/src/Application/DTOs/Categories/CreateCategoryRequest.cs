namespace Application.DTOs.Categories;

public class CreateCategoryRequest
{
    public Ulid UserId { get; set; } = Ulid.Empty!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
}

//Decisiones de diseño:

//Excluí Id porque se genera automáticamente en la BD (IDENTITY)
//Excluí CreatedAt porque se asigna automáticamente con DEFAULT GETDATE()
//Incluí solo los campos que el cliente debe proporcionar al crear una categoría
