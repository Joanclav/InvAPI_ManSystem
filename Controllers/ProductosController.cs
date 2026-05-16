using InventarioAPI.DTOs;
using InventarioAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioAPI.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductosController(IProductoService service)
    {
        _service = service;
    }

    /// <summary>Lista todos los productos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _service.GetAllAsync();
        return Ok(productos);
    }

    /// <summary>Obtiene un producto por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _service.GetByIdAsync(id);
        return producto is null ? NotFound(new { message = $"Producto con Id {id} no encontrado." }) : Ok(producto);
    }

    /// <summary>Crea un nuevo producto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualiza un producto existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { message = $"Producto con Id {id} no encontrado." }) : Ok(updated);
    }

    /// <summary>Elimina un producto (eliminación física).</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { message = $"Producto con Id {id} no encontrado." });
    }
}
