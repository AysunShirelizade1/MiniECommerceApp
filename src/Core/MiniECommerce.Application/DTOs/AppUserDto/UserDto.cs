using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.DTOs.AppUserDto;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;

    public UserDto(AppUser user, string role)
    {
        Id = user.Id;
        FullName = user.FullName;
        Email = user.Email;
    }
}