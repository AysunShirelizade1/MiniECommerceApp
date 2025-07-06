using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniECommerce.WebApi.Controllers;

[Authorize(Roles = "admin")]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    // ✅ Bütün rolları görmək
    [HttpGet]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        return Ok(roles);
    }

    // ✅ Yeni rol yaratmaq
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return BadRequest("Bu rol artıq mövcuddur.");

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
            return Ok("Rol yaradıldı.");

        return BadRequest(result.Errors);
    }

    // ✅ Rol silmək
    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return NotFound("Rol tapılmadı.");

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
            return Ok("Rol silindi.");

        return BadRequest(result.Errors);
    }
}
