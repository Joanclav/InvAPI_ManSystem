using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventarioAPI.Data;
using InventarioAPI.DTOs;
using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InventarioAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == dto.NombreUsuario);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return null;

        var token = GenerateJwtToken(usuario);
        return new LoginResponseDto(token, usuario.NombreUsuario, usuario.Rol);
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        var exists = await _context.Usuarios.AnyAsync(u => u.NombreUsuario == dto.NombreUsuario);
        if (exists) return false;

        var usuario = new Usuario
        {
            NombreUsuario = dto.NombreUsuario,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = dto.Rol
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpirationHours"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
