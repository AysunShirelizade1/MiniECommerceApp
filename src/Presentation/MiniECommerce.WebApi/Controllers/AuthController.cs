using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.AppUserDto;
using MiniECommerce.Application.DTOs.RefreshTokenDto;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Services;

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly JwtTokenService _jwtService;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        JwtTokenService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        dto.Role = char.ToUpper(dto.Role[0]) + dto.Role.Substring(1).ToLower();

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest("User already exists.");

        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!await _roleManager.RoleExistsAsync(dto.Role))
            await _roleManager.CreateAsync(new AppRole { Name = dto.Role });

        await _userManager.AddToRoleAsync(user, dto.Role);

        return Ok("Qeydiyyat uğurla tamamlandı.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("İstifadəçi tapılmadı");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Şifrə yalnışdır");

        var (accessToken, refreshToken) = await _jwtService.GenerateTokenWithRefreshToken(user, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");

        return Ok(new
        {
            accessToken,
            refreshToken
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenRequest request)
    {
        var newAccessToken = await _jwtService.RefreshAccessToken(request.Token);
        if (newAccessToken == null)
            return Unauthorized("Refresh token etibarsız və ya vaxtı keçib.");

        return Ok(new
        {
            accessToken = newAccessToken
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            Roles = roles
        });
    }
}
