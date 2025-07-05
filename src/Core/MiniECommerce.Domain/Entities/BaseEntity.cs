using System;

namespace MiniECommerce.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }

    // Audit fields
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? UpdatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Optional Navigation Properties (User-a FK varsa)
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
