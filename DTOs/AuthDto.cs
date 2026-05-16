using System.ComponentModel.DataAnnotations;

namespace InventarioAPI.DTOs;

public record LoginRequestDto(
    [Required] string NombreUsuario,
    [Required] string Password
);

public record LoginResponseDto(string Token, string NombreUsuario, string Rol);

public record RegisterRequestDto(
    [Required, MaxLength(50)] string NombreUsuario,
    [Required, MinLength(6)] string Password,
    string Rol = "User"
);
