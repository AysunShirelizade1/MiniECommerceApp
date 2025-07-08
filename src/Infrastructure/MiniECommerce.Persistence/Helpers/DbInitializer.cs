//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MiniECommerce.Domain.Entities;
//using MiniECommerce.Persistence.Contexts;

//namespace MiniECommerce.Persistence.Helpers;

//public static class DbInitializer
//{
//    public static async Task InitializeAsync(MiniECommerceDbContext context)
//    {
//        // DB yoxlanışı və yaranması
//        await context.Database.EnsureCreatedAsync();

//        // Rolların əlavə olunması
//        if (!context.Roles.Any())
//        {
//            context.Roles.AddRange(
//                new AppRole { Name = "Admin", NormalizedName = "ADMIN" },
//                new AppRole { Name = "Seller", NormalizedName = "SELLER" },
//                new AppRole { Name = "Moderator", NormalizedName = "MODERATOR" },
//                new AppRole { Name = "Buyer", NormalizedName = "BUYER" }
//            );
//            await context.SaveChangesAsync();
//        }

//        // İcazələrin əlavə olunması (Permissions)
//        if (!context.Permissions.Any())
//        {
//            context.Permissions.AddRange(
//                // Product icazələri
//                new Permission { Name = "Product.Create" },
//                new Permission { Name = "Product.Read" },
//                new Permission { Name = "Product.Update" },
//                new Permission { Name = "Product.Delete" },

//                // Category icazələri
//                new Permission { Name = "Category.Create" },
//                new Permission { Name = "Category.Read" },
//                new Permission { Name = "Category.Update" },
//                new Permission { Name = "Category.Delete" },

//                // Order icazələri
//                new Permission { Name = "Order.Create" },
//                new Permission { Name = "Order.Read" },
//                new Permission { Name = "Order.Update" },

//                // Image icazələri
//                new Permission { Name = "Image.Create" },
//                new Permission { Name = "Image.Update" },
//                new Permission { Name = "Image.Delete" },

//                // Review icazələri
//                new Permission { Name = "Review.Create" },
//                new Permission { Name = "Review.Delete" },

//                // User və Role idarəetmə icazələri
//                new Permission { Name = "User.Manage" },
//                new Permission { Name = "Role.Manage" },

//                // Statistikalar və idarə paneli
//                new Permission { Name = "Analytics.View" }
//            );
//            await context.SaveChangesAsync();
//        }

//        // RolePermissions münasibətlərinin əlavə olunması
//        if (!context.RolePermissions.Any())
//        {
//            var adminRole = context.Roles.First(r => r.Name == "Admin");
//            var sellerRole = context.Roles.First(r => r.Name == "Seller");
//            var moderatorRole = context.Roles.First(r => r.Name == "Moderator");
//            var buyerRole = context.Roles.First(r => r.Name == "Buyer");

//            var allPermissions = context.Permissions.ToList();

//            // Admin: Bütün icazələr
//            foreach (var permission in allPermissions)
//            {
//                context.RolePermissions.Add(new RolePermission
//                {
//                    RoleId = adminRole.Id,
//                    PermissionId = permission.Id
//                });
//            }

//            // Seller: Məhsullar və sifarişlərin idarəsi (sahib olduğu məhsullarla bağlı)
//            var sellerPermissions = context.Permissions.Where(p =>
//                p.Name.StartsWith("Product") ||
//                p.Name.StartsWith("Order.Read") ||
//                p.Name == "Image.Create" ||
//                p.Name == "Image.Update" ||
//                p.Name == "Image.Delete" ||
//                p.Name == "Review.Create" ||
//                p.Name == "Review.Delete"
//            ).ToList();

//            foreach (var permission in sellerPermissions)
//            {
//                context.RolePermissions.Add(new RolePermission
//                {
//                    RoleId = sellerRole.Id,
//                    PermissionId = permission.Id
//                });
//            }

//            // Moderator: Review və User idarəetməsi, məhsulların və kateqoriyaların oxunması və yenilənməsi
//            var moderatorPermissions = context.Permissions.Where(p =>
//                p.Name == "Review.Delete" ||
//                p.Name == "User.Manage" ||
//                p.Name == "Category.Read" ||
//                p.Name == "Category.Update" ||
//                p.Name == "Product.Read" ||
//                p.Name == "Product.Update"
//            ).ToList();

//            foreach (var permission in moderatorPermissions)
//            {
//                context.RolePermissions.Add(new RolePermission
//                {
//                    RoleId = moderatorRole.Id,
//                    PermissionId = permission.Id
//                });
//            }

//            // Buyer: Məhsulların oxunması və sifarişlərin yaradılması, rəylərin yaradılması
//            var buyerPermissions = context.Permissions.Where(p =>
//                p.Name == "Product.Read" ||
//                p.Name == "Order.Create" ||
//                p.Name == "Review.Create"
//            ).ToList();

//            foreach (var permission in buyerPermissions)
//            {
//                context.RolePermissions.Add(new RolePermission
//                {
//                    RoleId = buyerRole.Id,
//                    PermissionId = permission.Id
//                });
//            }

//            await context.SaveChangesAsync();
//        }
//    }
//}
