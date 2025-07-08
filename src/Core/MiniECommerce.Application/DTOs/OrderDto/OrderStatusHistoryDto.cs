using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Application.DTOs.OrderDto;

public class OrderStatusHistoryDto
{
    public string Status { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
}

