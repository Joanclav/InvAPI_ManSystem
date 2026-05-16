using InventarioAPI.DTOs;
using InventarioAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Genera un token JWT validando credenciales.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return result is null
            ? Unauthorized(new { message = "Credenciales inválidas." })
            : Ok(result);
    }

    /// <summary>Registra un nuevo usuario.</summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var success = await _authService.RegisterAsync(dto);
        return success
            ? StatusCode(StatusCodes.Status201Created, new { message = "Usuario registrado exitosamente." })
            : Conflict(new { message = $"El usuario '{dto.NombreUsuario}' ya existe." });
    }
}
