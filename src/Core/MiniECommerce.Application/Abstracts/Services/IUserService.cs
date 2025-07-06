using MiniECommerce.Application.DTOs.AppUserDto;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IUserService
{
    Task<TokenDto> LoginAsync(LoginDto loginDto);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid id);
}
