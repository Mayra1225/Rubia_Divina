using Rubia_Divina.DTOs;

namespace Rubia_Divina.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDTO dto);

    Task<AuthResponse> RegisterAsync(RegisterDTO dto);
}