using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly MiniECommerceDbContext _context;

    public OrderRepository(MiniECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Order?> GetOrderWithProductsAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetAllWithProductsAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();
    }
    public async Task<List<OrderStatusHistory>> GetOrderStatusHistoryAsync(Guid orderId)
    {
        return await _context.OrderStatusHistories
            .Where(h => h.OrderId == orderId)
            .OrderBy(h => h.ChangedAt)
            .ToListAsync();
    }

    public async Task AddOrderStatusHistoryAsync(OrderStatusHistory statusHistory)
    {
        await _context.OrderStatusHistories.AddAsync(statusHistory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order entity)
    {
        _context.Orders.Update(entity);
        await _context.SaveChangesAsync();
    }



}
