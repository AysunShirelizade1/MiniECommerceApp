//using MiniECommerce.Application.Services.Interfaces;
//using MiniECommerce.Domain.DTOs;
//using MiniECommerce.Domain.Models;
//using MiniECommerce.Persistence.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using MiniECommerceApp.Application.Services.Interfaces;

//namespace MiniECommerce.Application.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly MiniECommerceDbContext _context;
//        private readonly IConfiguration _configuration;

//        public AuthService(MiniECommerceDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        public async Task<ServiceResult<UserDto>> RegisterAsync(RegisterDto dto)
//        {
//            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
//            if (exists)
//                return ServiceResult<UserDto>.Fail("Email already in use.");

//            var user = new AppUser
//            {
//                Id = Guid.NewGuid(),
//                Email = dto.Email,
//                UserName = dto.Email,
//                FullName = dto.FullName,
//                Role = dto.Role,
//                PasswordHash = HashPassword(dto.Password)
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return ServiceResult<UserDto>.Success(new UserDto(user));
//        }

//        public async Task<ServiceResult<TokenDto>> LoginAsync(LoginDto dto)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
//            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
//                return ServiceResult<TokenDto>.Fail("Invalid credentials.");

//            var token = GenerateJwtToken(user);
//            return ServiceResult<TokenDto>.Success(new TokenDto { Token = token });
//        }

//        public async Task<ServiceResult<UserDto>> GetUserProfileAsync(Guid userId)
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//                return ServiceResult<UserDto>.Fail("User not found.");

//            return ServiceResult<UserDto>.Success(new UserDto(user));
//        }

//        private string HashPassword(string password)
//        {
//            // TODO: Hashing logic (e.g. BCrypt)
//            return password; // sadəcə nümunə üçün
//        }

//        private bool VerifyPassword(string password, string hash)
//        {
//            // TODO: Verify hashing logic
//            return password == hash; // sadəcə nümunə üçün
//        }

//        private string GenerateJwtToken(AppUser user)
//        {
//            var jwtSettings = _configuration.GetSection("Jwt");
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim("id", user.Id.ToString()),
//                new Claim(ClaimTypes.Email, user.Email),
//                new Claim(ClaimTypes.Role, user.Role)
//            };

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddHours(3),
//                signingCredentials: creds);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
