using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniECommerce.Application.DTOs.AppUserDto;
using MiniECommerce.Domain.Entities;
using MiniECommerceApp.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AppUser> userManager,
                       RoleManager<AppRole> roleManager,
                       IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
        if (userExists != null)
            return false;

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FullName = registerDto.FullName,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
            return false;

        // Əgər rol yoxdursa, yarat (GUID əsaslı AppRole ilə)
        if (!await _roleManager.RoleExistsAsync(registerDto.Role))
        {
            await _roleManager.CreateAsync(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = registerDto.Role,
                NormalizedName = registerDto.Role.ToUpper()
            });
        }

        await _userManager.AddToRoleAsync(user, registerDto.Role);

        return true;
    }

    public async Task<TokenDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return null;

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = GenerateJwtToken(authClaims);

        return new TokenDto { Token = new JwtSecurityTokenHandler().WriteToken(token) };
    }

    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
