using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Order;
using MiniECommerce.Application.DTOs.OrderProductItem;
using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MiniECommerce.Application.DTOs.OrderProduct;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderProduct> _orderProductRepository;
    private readonly IRepository<Product> _productRepository;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<OrderProduct> orderProductRepository,
        IRepository<Product> productRepository)
    {
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _productRepository = productRepository;
    }

    public async Task CreateAsync(OrderCreateDto dto)
    {
        var order = new Order
        {
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangeAsync();

        foreach (var item in dto.Products)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product with Id {item.ProductId} not found.");

            var orderProduct = new OrderProduct
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };

            await _orderProductRepository.AddAsync(orderProduct);
        }

        await _orderProductRepository.SaveChangeAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order != null)
        {
            _orderRepository.Delete(order);
            await _orderRepository.SaveChangeAsync();
        }
    }

    public async Task<IEnumerable<OrderListDto>> GetAllAsync()
    {
        var orders = _orderRepository.GetAllFiltered(
            include: new Expression<Func<Order, object>>[] { o => o.OrderProducts },
            isTracking: false);

        var list = await orders.Select(o => new OrderListDto
        {
            Id = o.Id,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            ProductCount = o.OrderProducts.Count
        }).ToListAsync();

        return list;
    }

    public async Task<OrderDetailDto?> GetByIdAsync(Guid id)
    {
        var orderQuery = _orderRepository.GetByFiltered(
            o => o.Id == id,
            include: new Expression<Func<Order, object>>[] { o => o.Buyer, o => o.OrderProducts },
            isTracking: false);

        var order = await orderQuery.FirstOrDefaultAsync();

        if (order == null) return null;

        var products = order.OrderProducts.Select(op => new OrderProductDto
        {
            ProductId = op.ProductId,
            Quantity = op.Quantity,
            UnitPrice = op.UnitPrice
        }).ToList();

        return new OrderDetailDto
        {
            Id = order.Id,
            Status = order.Status,
            BuyerId = order.BuyerId,
            BuyerName = order.Buyer.FullName,
            CreatedAt = order.CreatedAt,
            Products = products
        };
    }

    public async Task UpdateAsync(OrderUpdateDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);
        if (order == null) return;

        order.Status = dto.Status;

        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();
    }
}
