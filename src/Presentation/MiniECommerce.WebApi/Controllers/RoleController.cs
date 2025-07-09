using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Domain.Entities; 
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly MiniECommerceDbContext _context;

    public RoleController(RoleManager<AppRole> roleManager, MiniECommerceDbContext context)
    {
        _roleManager = roleManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.NormalizedName
            })
            .ToList();

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

    // Yeni: Verilmiş role aid icazələri gətirir
    [HttpGet("{roleId}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            return NotFound("Rol tapılmadı.");

        var permissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(rp => new
            {
                rp.Permission.Id,
                rp.Permission.Name
            })
            .ToListAsync();

        return Ok(new
        {
            Role = new { role.Id, role.Name },
            Permissions = permissions
        });
    }

    // Yeni: Role icazə əlavə edir
    [HttpPost("{roleId}/permissions")]
    public async Task<IActionResult> AddPermissions(Guid roleId, [FromBody] List<string> permissionNames)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            return NotFound("Rol tapılmadı.");

        var allPermissions = await _context.Permissions
            .Where(p => permissionNames.Contains(p.Name))
            .ToListAsync();

        foreach (var permission in allPermissions)
        {
            bool exists = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleId == roleId && rp.PermissionId == permission.Id);

            if (!exists)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission.Id
                });
            }
        }

        await _context.SaveChangesAsync();

        return Ok("Permissions əlavə olundu.");
    }

    // Yeni: Role aid icazəni silir
    [HttpDelete("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermission(Guid roleId, Guid permissionId)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (rolePermission == null)
            return NotFound("Role və permission əlaqəsi tapılmadı.");

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();

        return Ok("Permission roldan silindi.");
    }
}
