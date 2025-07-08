using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Order;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private Guid? GetUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdStr, out var userId) ? userId : null;
    }

    [HttpPost]
    [Authorize(Policy = "Order.Create")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var orderId = await _orderService.CreateAsync(dto, userId.Value);
        return Ok(new { OrderId = orderId });
    }

    [HttpGet("my")]
    [Authorize(Policy = "Order.Read")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var orders = await _orderService.GetOrdersByBuyerIdAsync(userId.Value);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Order.Read")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();

        if (order.BuyerId != userId)
        {
            var sales = await _orderService.GetMySalesAsync(userId.Value);
            if (!sales.Any(s => s.Id == id))
                return Forbid();
        }

        return Ok(order);
    }

    [HttpGet("{id}/history")]
    [Authorize(Policy = "Order.Read")]
    public async Task<IActionResult> GetStatusHistory(Guid id)
    {
        var histories = await _orderService.GetStatusHistoryAsync(id);
        if (histories == null || !histories.Any()) return NotFound();

        return Ok(histories);
    }

    [HttpPut("{id}/cancel")]
    [Authorize(Policy = "Order.Cancel")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        try
        {
            await _orderService.CancelOrderAsync(id);
            return Ok("Order cancelled successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("my-sales")]
    [Authorize(Policy = "Order.Read")]
    public async Task<IActionResult> GetMySales()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var sales = await _orderService.GetMySalesAsync(userId.Value);
        return Ok(sales);
    }

    [HttpPut("status")]
    [Authorize(Policy = "Order.Update")]
    public async Task<IActionResult> UpdateStatus([FromBody] OrderUpdateDto dto)
    {
        try
        {
            await _orderService.UpdateStatusAsync(dto);
            return Ok("Order status updated.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
