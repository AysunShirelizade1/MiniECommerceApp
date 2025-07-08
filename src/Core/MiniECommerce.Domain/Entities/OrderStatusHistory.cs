using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Domain.Entities;

public class OrderStatusHistory
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public Order Order { get; set; } = null!;
}

