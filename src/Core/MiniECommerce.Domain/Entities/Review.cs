namespace MiniECommerce.Domain.Entities;

public class Review : BaseEntity
{
    public string Comment { get; set; }
    public int Rating { get; set; }


    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}

