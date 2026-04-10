using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Helpers;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly PasswordHelper _passwordHelper;

    public AuthService(AppDbContext context, JwtHelper jwtHelper, PasswordHelper passwordHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _passwordHelper = passwordHelper;
    }

    public async Task<AuthResponse> LoginAsync(LoginDTO dto)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email.Trim().ToLower());

        if (user is null || !_passwordHelper.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Credenciales incorrectas."
            };
        }

        return new AuthResponse
        {
            Success = true,
            Message = "Inicio de sesión exitoso.",
            Token = _jwtHelper.GenerateToken(user),
            Email = user.Email,
            Nombre = user.Nombre
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterDTO dto)
    {
        var email = dto.Email.Trim().ToLower();
        var exists = await _context.Usuarios.AnyAsync(u => u.Email == email);

        if (exists)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Ya existe un usuario registrado con ese correo."
            };
        }

        var user = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Email = email,
            PasswordHash = _passwordHelper.HashPassword(dto.Password)
        };

        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Success = true,
            Message = "Registro exitoso.",
            Token = _jwtHelper.GenerateToken(user),
            Email = user.Email,
            Nombre = user.Nombre
        };
    }
}
