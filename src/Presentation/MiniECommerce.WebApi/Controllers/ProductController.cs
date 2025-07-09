using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Product;
using System.Security.Claims;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // Everyone can view products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // Only authorized users with Product.Create permission
    [HttpPost]
    [Authorize(Policy = "Product.Create")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim.Value);
        await _productService.CreateAsync(dto, userId);

        return CreatedAtAction(nameof(GetById), new { id = dto.CategoryId }, dto);
    }

    // Only Product.Update permission holders (Admin, Moderator, Seller with policy)
    [HttpPut("{id}")]
    [Authorize(Policy = "Product.Update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
    {
        await _productService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpGet("myproducts")]
    [Authorize]  // yalnız authorized istifadəçilər
    public async Task<IActionResult> GetMyProducts()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim.Value);
        var products = await _productService.GetProductsByUserIdAsync(userId);

        return Ok(products);
    }

    // Only Product.Delete permission holders
    [HttpDelete("{id}")]
    [Authorize(Policy = "Product.Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
