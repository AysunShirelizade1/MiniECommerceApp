using Microsoft.EntityFrameworkCore;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Helpers;

public static class DbInitializer
{
    public static async Task InitializeAsync(MiniECommerceDbContext context)
    {
        await AddRolesAsync(context);
        await AddPermissionsAsync(context);
        await AssignPermissionsToRolesAsync(context);
    }

    private static async Task AddRolesAsync(MiniECommerceDbContext context)
    {
        var roleNames = new[] { "Admin", "Seller", "Moderator", "Buyer" };

        foreach (var roleName in roleNames)
        {
            if (!context.Roles.Any(r => r.NormalizedName == roleName.ToUpper()))
            {
                var newRole = new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                await context.Roles.AddAsync(newRole);
            }
        }

        await context.SaveChangesAsync();
    }


    private static async Task AddPermissionsAsync(MiniECommerceDbContext context)
    {
        var permissionNames = new[]
        {
            // Product
            "Product.Create", "Product.Read", "Product.Update", "Product.Delete",

            // Order
            "Order.Read", "Order.Cancel",

            // Image
            "Image.Create", "Image.Update", "Image.Delete",

            // Review
            "Review.Create", "Review.Delete"
        };

        foreach (var name in permissionNames)
        {
            if (!context.Permissions.Any(p => p.Name == name))
            {
                await context.Permissions.AddAsync(new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = name
                });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task AssignPermissionsToRolesAsync(MiniECommerceDbContext context)
    {
        Console.WriteLine("=== AssignPermissionsToRolesAsync başladı ===");

        // Bu 2 blokun əlavə olunması vacibdir!
        var dbRoles = await context.Roles.ToListAsync();
        var roles = dbRoles.ToDictionary(r => r.Name!.ToLower(), r => r.Id);
        Console.WriteLine($"Toplam {roles.Count} rol tapıldı:");
        foreach (var r in roles)
        {
            Console.WriteLine($" - {r.Key} => {r.Value}");
        }

        var dbPermissions = await context.Permissions.ToListAsync();
        var permissions = dbPermissions.ToDictionary(p => p.Name!, p => p.Id);
        Console.WriteLine($"Toplam {permissions.Count} permission tapıldı.");

        void Assign(string roleName, IEnumerable<string> permissionNames)
        {
            Console.WriteLine($"-> Rol təyini: {roleName}");

            var roleKey = roleName.ToLower();

            if (!roles.ContainsKey(roleKey))
            {
                Console.WriteLine($"!!! ROL TAPILMADI: {roleName}");
                throw new Exception($"Role '{roleName}' not found in the database.");
            }

            foreach (var perm in permissionNames)
            {
                if (permissions.TryGetValue(perm, out var permId))
                {
                    bool alreadyExists = context.RolePermissions.Any(rp =>
                        rp.RoleId == roles[roleKey] && rp.PermissionId == permId);

                    if (!alreadyExists)
                    {
                        Console.WriteLine($" + Permission əlavə olunur: {perm} → {roleName}");
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = roles[roleKey],
                            PermissionId = permId
                        });
                    }
                    else
                    {
                        Console.WriteLine($" = Permission artıq var: {perm} → {roleName}");
                    }
                }
                else
                {
                    Console.WriteLine($"!!! Permission tapılmadı: {perm}");
                }
            }
        }

        Assign("Admin", permissions.Keys);
        Assign("Seller", new[]
        {
        "Product.Create", "Product.Read", "Product.Update", "Product.Delete",
        "Order.Read", "Order.Cancel",
        "Image.Create", "Image.Update", "Image.Delete",
        "Review.Create", "Review.Delete"
    });

        Assign("Moderator", new[]
        {
        "Product.Read",
        "Order.Read",
        "Review.Delete"
    });

        Assign("Buyer", new[]
        {
        "Order.Read",
        "Order.Cancel",
        "Review.Create"
    });

        await context.SaveChangesAsync();

        Console.WriteLine("=== AssignPermissionsToRolesAsync tamamlandı ===");
    }
}
