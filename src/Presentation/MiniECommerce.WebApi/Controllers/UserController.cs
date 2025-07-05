using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.AppUserDto;
using MiniECommerceApp.Domain.Entities;
namespace MiniECommerce.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Moderator")] // Məsələn, yalnız Admin və Moderatorlara açıqdır
public class UserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/User
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "No Role";

            userDtos.Add(new UserDto(user, role));
        }

        return Ok(userDtos);
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("İstifadəçi tapılmadı.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "No Role";

        var userDto = new UserDto(user, role);
        return Ok(userDto);
    }

    // POST: api/User
    // Yeni istifadəçi əlavə etmək üçün (Admin və ya Moderator ola bilər)
    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest("Bu istifadəçi artıq mövcuddur.");

        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Rol yaradılmayıbsa yaradılır və istifadəçiyə əlavə edilir
        if (!await _userManager.IsInRoleAsync(user, dto.Role))
        {
            await _userManager.AddToRoleAsync(user, dto.Role);
        }

        return Ok("İstifadəçi uğurla yaradıldı.");
    }
}
