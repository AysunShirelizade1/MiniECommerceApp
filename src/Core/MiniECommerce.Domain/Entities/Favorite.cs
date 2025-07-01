using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Domain.Entities;

public class Favourite : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
}
