using Microsoft.AspNetCore.Identity;

namespace MiniECommerce.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;


}
