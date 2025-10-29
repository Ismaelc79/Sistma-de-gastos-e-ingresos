using Application.DTOs.Categories;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetCategoriesByUserIdAsync(string userId);
    Task<IEnumerable<CategoryDto>> GetCategoriesByTypeAsync(string userId, string type);
    Task<CategoryDto> UpdateCategoryAsync(int id, CreateCategoryRequest request);
    Task<bool> DeleteCategoryAsync(int id);
}
