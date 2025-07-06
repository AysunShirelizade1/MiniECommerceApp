using Microsoft.AspNetCore.Identity;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.AccountDto;
using MiniECommerce.Application.DTOs.AppUserDto;
using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Persistence.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;

    public AccountService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null!;

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return new UserDto(user, role);
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        user.UserName = dto.UserName;
        user.Email = dto.Email;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        return result.Succeeded;
    }
}
