using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Email;
using MiniECommerce.Application.DTOs.Order;
using MiniECommerce.Application.DTOs.OrderProduct;

namespace MiniECommerce.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IEmailService emailService,
        IUserService userService) 
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _emailService = emailService;
        _userService = userService;
    }


    public async Task<List<OrderListDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllWithProductsAsync();

        return orders.Select(o => new OrderListDto
        {
            Id = o.Id,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            ProductCount = o.OrderProducts.Count
        }).ToList();
    }

    public async Task<OrderDetailDto?> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetOrderWithProductsAsync(id);
        if (order == null) return null;

        return new OrderDetailDto
        {
            Id = order.Id,
            Status = order.Status,
            BuyerId = order.BuyerId,
            BuyerName = order.Buyer.FullName,
            CreatedAt = order.CreatedAt,
            Products = order.OrderProducts.Select(op => new OrderProductDto
            {
                ProductId = op.ProductId,
                Title = op.Product.Title,
                Quantity = op.Quantity,
                UnitPrice = op.UnitPrice
            }).ToList()
        };
    }

    public async Task<Guid> CreateAsync(OrderCreateDto dto, Guid buyerId)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            BuyerId = buyerId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            OrderProducts = new List<OrderProduct>()
        };

        foreach (var item in dto.Products)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product with Id {item.ProductId} not found.");

            order.OrderProducts.Add(new OrderProduct
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangeAsync();

        // İstifadəçinin emailini alırıq
        var buyerEmail = await _userService.GetEmailByIdAsync(buyerId);
        if (!string.IsNullOrEmpty(buyerEmail))
        {
            await _emailService.SendAsync(new EmailDto
            {
                To = "sunahacker01@gmail.com",
                Subject = "Sifariş təsdiqi",
                Body = $"Sifarişiniz (ID: {order.Id}) qəbul edildi. Ümumi məhsul sayı: {order.OrderProducts.Count}."
            });

        }

        return order.Id;
    }




    public async Task UpdateStatusAsync(OrderUpdateDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);
        if (order == null)
            throw new Exception("Order not found.");

        order.Status = dto.Status;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new Exception("Order not found.");

        _orderRepository.Delete(order);
        await _orderRepository.SaveChangeAsync();
    }
}
