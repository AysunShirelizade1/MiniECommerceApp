using Microsoft.AspNetCore.Identity;

namespace MiniECommerceApp.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;


}
