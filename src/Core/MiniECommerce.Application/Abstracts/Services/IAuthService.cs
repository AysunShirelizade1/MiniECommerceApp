using MiniECommerce.Application.DTOs.AppUserDto;

public interface IAuthService
{
    Task<TokenDto?> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterAsync(RegisterDto registerDto);
}
