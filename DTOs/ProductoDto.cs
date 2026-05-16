using System.ComponentModel.DataAnnotations;

namespace InventarioAPI.DTOs;

public record ProductoCreateDto(
    [Required, MaxLength(100)] string Nombre,
    [MaxLength(255)] string? Descripcion,
    [Range(0, int.MaxValue)] int Stock,
    [Range(0.01, double.MaxValue)] decimal Precio
);

public record ProductoUpdateDto(
    [Required, MaxLength(100)] string Nombre,
    [MaxLength(255)] string? Descripcion,
    [Range(0, int.MaxValue)] int Stock,
    [Range(0.01, double.MaxValue)] decimal Precio
);

public record ProductoResponseDto(
    int Id,
    string Nombre,
    string? Descripcion,
    int Stock,
    decimal Precio,
    DateTime FechaCreacion
);
