namespace MiniECommerce.Domain.Entities;

public class Permission
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!; // Məs: "User.Read", "Product.Create"

    public ICollection<RolePermission> RolePermissions { get; set; }
}
