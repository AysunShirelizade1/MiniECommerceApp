//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MiniECommerce.Application.Services;
//using MiniECommerce.Domain.DTOs;

//namespace MiniECommerce.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductsController : ControllerBase
//    {
//        private readonly IProductService _productService;

//        public ProductsController(IProductService productService)
//        {
//            _productService = productService;
//        }

//        // Hamı görə bilər
//        [HttpGet]
//        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDto filter)
//        {
//            var products = await _productService.GetAllAsync(filter);
//            return Ok(products);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(Guid id)
//        {
//            var product = await _productService.GetByIdAsync(id);
//            if (product == null)
//                return NotFound();

//            return Ok(product);
//        }

//        // Yalnız Seller rolundakılar əlavə edə bilər
//        [Authorize(Roles = "Seller")]
//        [HttpPost]
//        public async Task<IActionResult> Create(ProductCreateDto dto)
//        {
//            var userId = Guid.Parse(User.FindFirst("id")?.Value ?? string.Empty);
//            var result = await _productService.CreateAsync(dto, userId);
//            if (!result.Success)
//                return BadRequest(result.Message);

//            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
//        }

//        // Yalnız sahibi redaktə edə bilər
//        [Authorize(Roles = "Seller")]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(Guid id, ProductUpdateDto dto)
//        {
//            var userId = Guid.Parse(User.FindFirst("id")?.Value ?? string.Empty);
//            var result = await _productService.UpdateAsync(id, dto, userId);
//            if (!result.Success)
//                return BadRequest(result.Message);

//            return Ok(result.Data);
//        }

//        // Yalnız sahibi silə bilər
//        [Authorize(Roles = "Seller")]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var userId = Guid.Parse(User.FindFirst("id")?.Value ?? string.Empty);
//            var result = await _productService.DeleteAsync(id, userId);
//            if (!result.Success)
//                return BadRequest(result.Message);

//            return NoContent();
//        }

//        // Seller öz məhsullarını görə bilər
//        [Authorize(Roles = "Seller")]
//        [HttpGet("my")]
//        public async Task<IActionResult> GetMyProducts()
//        {
//            var userId = Guid.Parse(User.FindFirst("id")?.Value ?? string.Empty);
//            var products = await _productService.GetByOwnerIdAsync(userId);
//            return Ok(products);
//        }
//    }
//}
