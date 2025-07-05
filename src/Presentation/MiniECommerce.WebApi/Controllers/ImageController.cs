using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Image;

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    // GET api/image/product/{productId}
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetAllByProductId(Guid productId)
    {
        var images = await _imageService.GetAllByProductIdAsync(productId);
        return Ok(images);
    }

    // GET api/image/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var image = await _imageService.GetByIdAsync(id);
        if (image == null) return NotFound();
        return Ok(image);
    }

    // POST api/image
    [HttpPost]
    [Authorize]  // İstifadəçi login olmalıdır
    public async Task<IActionResult> Create([FromBody] CreateImageDto dto)
    {
        await _imageService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = dto.ProductId }, null);
    }

    // PUT api/image/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateImageDto dto)
    {
        try
        {
            await _imageService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE api/image/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _imageService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
