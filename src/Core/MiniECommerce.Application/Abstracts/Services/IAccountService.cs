using MiniECommerce.Application.DTOs.AccountDto;
using MiniECommerce.Application.DTOs.AppUserDto;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IAccountService
{
    Task<UserDto> GetProfileAsync(Guid userId);
    Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
}
