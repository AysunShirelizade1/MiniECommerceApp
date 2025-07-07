using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace MiniECommerce.Persistence.Services;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }
    public PermissionRequirement(string permissionName)
    {
        PermissionName = permissionName;
    }
}

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // Əgər istifadəçi Admin rolundadırsa, avtomatik uğurlu say
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var permissions = context.User.FindAll("permission").Select(c => c.Value);

        if (permissions.Contains(requirement.PermissionName))
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
