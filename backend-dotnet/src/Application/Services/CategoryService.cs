using Application.DTOs.Categories;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);

        var createdCategory = await _unitOfWork.Category.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(createdCategory);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id);
        return category != null ? _mapper.Map<CategoryDto>(category) : null;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesByUserIdAsync(Ulid userId)
    {
        var categories = await _unitOfWork.Category.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesByTypeAsync(Ulid userId, string type)
    {
        var categories = await _unitOfWork.Category.GetByUserIdAsync(userId);
        var filtered = categories.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        return _mapper.Map<IEnumerable<CategoryDto>>(filtered);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, CreateCategoryRequest request)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Categoría con ID {id} no encontrada");

        category.EditCategory(request.Name, request.Type);

        var updatedCategory = await _unitOfWork.Category.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(updatedCategory);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        if (await _unitOfWork.Category.DeleteAsync(id))
        {
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        else throw new Exception($"No fue posible eliminar la categoría con ID {id}"); ;
    }
}
