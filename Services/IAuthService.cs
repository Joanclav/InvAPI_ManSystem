using InventarioAPI.DTOs;

namespace InventarioAPI.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
    Task<bool> RegisterAsync(RegisterRequestDto dto);
}
