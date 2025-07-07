﻿namespace MiniECommerce.Domain.Entities;

public class RolePermission
{
    public Guid RoleId { get; set; }
    public AppRole Role { get; set; } = null!;

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
