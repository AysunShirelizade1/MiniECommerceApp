namespace MiniECommerce.Application.DTOs.AppUserDto;

public class RegisterDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;  // "Admin", "Seller", "Buyer", "Moderator"
}