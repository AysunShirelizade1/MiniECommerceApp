using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;  // DbContext üçün
using System.Linq;
using System.Threading.Tasks;

namespace MiniECommerce.Persistence.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly MiniECommerceDbContext _dbContext;

    public JwtTokenService(
        IConfiguration configuration,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        MiniECommerceDbContext dbContext)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task<string> GenerateToken(AppUser user)
    {
        try
        {
            var rolesFromDb = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            // Rolları claim kimi əlavə et
            foreach (var role in rolesFromDb)
            {
                claims.Add(new Claim(ClaimTypes.Role, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(role)));
            }

            // İndi rollara bağlı permission-ları DB-dən alıb əlavə et
            var permissions = new List<string>();

            foreach (var roleName in rolesFromDb)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var rolePermissions = _dbContext.RolePermissions
                    .Where(rp => rp.RoleId == role.Id)
                    .Select(rp => rp.Permission.Name)
                    .ToList();

                permissions.AddRange(rolePermissions);
            }

            // Unique permission claim-lər əlavə et
            foreach (var permission in permissions.Distinct())
            {
                claims.Add(new Claim("permission", permission));
            }

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT Key boşdur.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Token yaratma xətası: " + ex.Message);
            throw;
        }
    }
}
