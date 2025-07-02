//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MiniECommerce.Application.Abstractions.Services;
//using MiniECommerce.Application.DTOs.Order;
//using MiniECommerce.Application.Services.Interfaces;

//namespace MiniECommerce.WebApi.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class OrderController : ControllerBase
//{
//    private readonly IOrderService _orderService;

//    public OrderController(IOrderService orderService)
//    {
//        _orderService = orderService;
//    }

//    [HttpGet]
//    [Authorize(Roles = "Admin,Moderator")]
//    public async Task<IActionResult> GetAll()
//    {
//        var orders = await _orderService.GetAllAsync();
//        return Ok(orders);
//    }

//    [HttpGet("{id}")]
//    [Authorize]
//    public async Task<IActionResult> GetById(Guid id)
//    {
//        var order = await _orderService.GetByIdAsync(id);
//        if (order == null) return NotFound();
//        return Ok(order);
//    }

//    [HttpPut("update-status")]
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> UpdateStatus([FromBody] OrderUpdateDto dto)
//    {
//        var result = await _orderService.UpdateStatusAsync(dto);
//        if (!result) return NotFound();
//        return Ok(new { message = "Order status updated." });
//    }
//}
