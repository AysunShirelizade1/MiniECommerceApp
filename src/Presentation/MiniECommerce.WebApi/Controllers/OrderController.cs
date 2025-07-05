using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.DTOs.Order;
using MiniECommerce.Application.DTOs.OrderProductItem;
using MiniECommerce.Application.DTOs.OrderProduct;
using MiniECommerceApp.Domain.Entities;
using MiniECommerceApp.Persistence.Contexts;
using System.Security.Claims;

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly MiniECommerceDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public OrderController(MiniECommerceDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // POST: /api/orders
    [HttpPost]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var productIds = dto.Products.Select(p => p.ProductId).ToList();

        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
            return NotFound("Bəzi məhsullar tapılmadı.");

        var orderProducts = dto.Products.Select(item =>
        {
            var product = products.First(p => p.Id == item.ProductId);
            return new OrderProduct
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };
        }).ToList();

        var order = new Order
        {
            BuyerId = userId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            OrderProducts = orderProducts
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new { OrderId = order.Id });
    }

    // GET: /api/orders/my
    [HttpGet("my")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var orders = await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Where(o => o.BuyerId == userId)
            .ToListAsync();

        var result = orders.Select(o => new OrderListDto
        {
            Id = o.Id,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            ProductCount = o.OrderProducts.Count
        });

        return Ok(result);
    }

    // GET: /api/orders/my-sales
    [HttpGet("my-sales")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> GetMySales()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var orders = await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Where(o => o.OrderProducts.Any(op => op.Product.OwnerId == userId))
            .ToListAsync();

        var result = orders.Select(o => new OrderListDto
        {
            Id = o.Id,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            ProductCount = o.OrderProducts.Count
        });

        return Ok(result);
    }

    // GET: /api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var order = await _context.Orders
            .Include(o => o.Buyer)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound("Sifariş tapılmadı.");

        var isSeller = order.OrderProducts.Any(op => op.Product.OwnerId == userId);
        if (order.BuyerId != userId && !isSeller)
            return Forbid("Bu sifarişi görmək icazən yoxdur.");

        var dto = new OrderDetailDto
        {
            Id = order.Id,
            BuyerId = order.BuyerId,
            BuyerName = order.Buyer.FullName,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Products = order.OrderProducts.Select(op => new OrderProductDto
            {
                ProductId = op.ProductId,
                Title = op.Product.Title,
                Quantity = op.Quantity,
                UnitPrice = op.UnitPrice
            }).ToList()
        };

        return Ok(dto);
    }

    // PUT: /api/orders/status
    [HttpPut("status")]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<IActionResult> UpdateStatus([FromBody] OrderUpdateDto dto)
    {
        var order = await _context.Orders.FindAsync(dto.OrderId);
        if (order == null)
            return NotFound("Sifariş tapılmadı.");

        order.Status = dto.Status;
        await _context.SaveChangesAsync();

        return Ok("Sifariş statusu yeniləndi.");
    }
}
