using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Services;
using MiniECommerceApp.Application.DTOs.Product;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET /api/products?categoryId=3&minPrice=10&maxPrice=100&search=keyboard
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] Guid? categoryId, [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice, [FromQuery] string? search)
    {
        // Burada filter və sort əlavə etmək üçün IProductService metodunu genişləndirə bilərsən.
        var products = await _productService.GetAllFilteredAsync(categoryId, minPrice, maxPrice, search);
        return Ok(products);
    }

    // GET /api/products/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST /api/products
    // Yalnız "Seller" rolu olan istifadəçi əlavə edə bilər
    [HttpPost]
    [Authorize(Roles = "Seller")]
    [HttpPost]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        var productId = await _productService.CreateAsync(dto, userId.Value);
        return CreatedAtAction(nameof(GetById), new { id = productId }, null);
    }


    // PUT /api/products/{id}
    // Yalnız məhsul sahibi redaktə edə bilər
    [HttpPut("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Update(Guid id, ProductUpdateDto dto)
    {
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        var success = await _productService.UpdateAsync(id, dto, userId.Value);
        if (!success)
            return Forbid();  // ya NotFound() əgər məhsul yoxdur

        return NoContent();
    }

    // DELETE /api/products/{id}
    // Yalnız məhsul sahibi silə bilər
    [HttpDelete("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        var success = await _productService.DeleteAsync(id, userId.Value);
        if (!success)
            return Forbid(); // ya NotFound()

        return NoContent();
    }

    // GET /api/products/my
    // Aktiv istifadəçinin öz məhsullarını gətirir
    [HttpGet("my")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> GetMyProducts()
    {
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        var products = await _productService.GetProductsByOwnerAsync(userId.Value);
        return Ok(products);
    }

    private Guid? GetUserIdFromToken()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdStr, out var userId))
            return userId;

        return null;
    }
}
