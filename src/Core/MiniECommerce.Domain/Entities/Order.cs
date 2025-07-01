namespace MiniECommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Status { get; set; }

        public Guid BuyerId { get; set; }           // FK olaraq lazımdır
        public User Buyer { get; set; }             // Navigation

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
