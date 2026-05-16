using InventarioAPI.Data;
using InventarioAPI.DTOs;
using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioAPI.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductoResponseDto>> GetAllAsync()
    {
        return await _context.Productos
            .AsNoTracking()
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    public async Task<ProductoResponseDto?> GetByIdAsync(int id)
    {
        var producto = await _context.Productos.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return producto is null ? null : ToDto(producto);
    }

    public async Task<ProductoResponseDto> CreateAsync(ProductoCreateDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Stock = dto.Stock,
            Precio = dto.Precio,
            FechaCreacion = DateTime.UtcNow
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return ToDto(producto);
    }

    public async Task<ProductoResponseDto?> UpdateAsync(int id, ProductoUpdateDto dto)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is null) return null;

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Stock = dto.Stock;
        producto.Precio = dto.Precio;

        await _context.SaveChangesAsync();

        return ToDto(producto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is null) return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ProductoResponseDto ToDto(Producto p) =>
        new(p.Id, p.Nombre, p.Descripcion, p.Stock, p.Precio, p.FechaCreacion);
}
