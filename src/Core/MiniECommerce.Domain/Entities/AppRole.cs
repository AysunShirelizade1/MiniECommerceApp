using Microsoft.AspNetCore.Identity;

namespace MiniECommerce.Domain.Entities;

public class AppRole : IdentityRole<Guid>
{
    public ICollection<RolePermission> RolePermissions { get; set; }
}

