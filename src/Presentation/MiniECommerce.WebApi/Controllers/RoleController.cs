using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Domain.Entities; // AppRole burada yerləşir

namespace MiniECommerce.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleController(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return BadRequest("Bu rol artıq mövcuddur.");

        var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });
        if (result.Succeeded)
            return Ok("Rol yaradıldı.");

        return BadRequest(result.Errors);
    }

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
