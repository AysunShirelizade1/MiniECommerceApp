using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Product;

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

    // GET: api/product
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    // GET: api/product/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/product
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = dto.CategoryId }, dto);
    }

    // PUT: api/product
    [HttpPut]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
    {
        await _productService.UpdateAsync(id, dto);
        return NoContent();
    }

    // DELETE: api/product/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
