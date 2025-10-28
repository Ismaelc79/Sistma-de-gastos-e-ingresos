namespace Application.DTOs.Categories;

public class CreateCategoryRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

//Decisiones de diseño:

//Excluí Id porque se genera automáticamente en la BD (IDENTITY)
//Excluí CreatedAt porque se asigna automáticamente con DEFAULT GETDATE()
//Incluí solo los campos que el cliente debe proporcionar al crear una categoría
