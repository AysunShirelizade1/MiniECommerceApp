using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerceApp.Application.Abstract;
using MiniECommerceApp.Application.DTOs.Product;
using System.Security.Claims;

namespace MiniECommerceApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            await _productService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = dto.CategoryId }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update(Guid id, ProductUpdateDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var result = await _productService.UpdateAsync(id, dto, userId);
            if (!result)
                return BadRequest("Update failed or you are not the owner of the product.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var result = await _productService.DeleteAsync(id, userId);
            if (!result)
                return BadRequest("Delete failed or you are not the owner of the product.");

            return NoContent();
        }
    }
}
