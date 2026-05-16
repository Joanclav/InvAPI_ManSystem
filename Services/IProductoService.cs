using InventarioAPI.DTOs;

namespace InventarioAPI.Services;

public interface IProductoService
{
    Task<IEnumerable<ProductoResponseDto>> GetAllAsync();
    Task<ProductoResponseDto?> GetByIdAsync(int id);
    Task<ProductoResponseDto> CreateAsync(ProductoCreateDto dto);
    Task<ProductoResponseDto?> UpdateAsync(int id, ProductoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
