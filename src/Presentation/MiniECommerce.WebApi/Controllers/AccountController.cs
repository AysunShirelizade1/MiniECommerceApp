using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Application.DTOs.AccountDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: /api/account/profile
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email
        });
    }

    // PUT: /api/account/profile
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        // Email yoxlaması
        if (user.Email != dto.Email)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
                return BadRequest("Bu email artıq başqa istifadəçi tərəfindən istifadə olunur.");
            user.Email = dto.Email;
            user.EmailConfirmed = false; // Email təsdiqi sistemi varsa
        }

        // UserName yoxlaması
        if (user.UserName != dto.UserName)
        {
            var existingUserByName = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUserByName != null && existingUserByName.Id != user.Id)
                return BadRequest("Bu istifadəçi adı artıq mövcuddur.");
            user.UserName = dto.UserName;
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Profil yeniləndi.");
    }


    // PUT: /api/account/change-password
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Şifrə uğurla dəyişdirildi.");
    }
}
