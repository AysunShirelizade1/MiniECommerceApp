using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

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
        var rolesFromDb = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        foreach (var role in rolesFromDb)
            claims.Add(new Claim(ClaimTypes.Role, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(role)));

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

        foreach (var permission in permissions.Distinct())
            claims.Add(new Claim("permission", permission));

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

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = DateTime.UtcNow.AddDays(7),
            UserId = Guid.Empty, // Sonra controllerdə təyin olunacaq
            IsRevoked = false
        };
    }

    public async Task<(string AccessToken, string RefreshToken)> GenerateTokenWithRefreshToken(AppUser user, string ipAddress)
    {
        var accessToken = await GenerateToken(user);

        var refreshToken = GenerateRefreshToken(ipAddress);
        refreshToken.UserId = user.Id;

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return (accessToken, refreshToken.Token);
    }

    public async Task<string?> RefreshAccessToken(string refreshToken)
    {
        var tokenFromDb = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken && !r.IsRevoked);

        if (tokenFromDb == null || tokenFromDb.Expires < DateTime.UtcNow)
            return null;

        var user = await _userManager.FindByIdAsync(tokenFromDb.UserId.ToString());
        if (user == null) return null;

        // Köhnə token-i etibarsız et
        tokenFromDb.IsRevoked = true;

        var newAccessToken = await GenerateToken(user);

        var newRefreshToken = GenerateRefreshToken("system");
        newRefreshToken.UserId = user.Id;
        _dbContext.RefreshTokens.Add(newRefreshToken);

        await _dbContext.SaveChangesAsync();

        return newAccessToken;
    }
}
