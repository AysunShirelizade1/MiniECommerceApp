using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.Image;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    // GET: api/image
    [HttpGet]
    public IActionResult GetAll()
    {
        var images = new List<ImageDto>
        {
            new ImageDto
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://example.com/image1.jpg",
                IsMain = true
            },
            new ImageDto
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://example.com/image2.jpg",
                IsMain = false
            }
        };

        return Ok(images);
    }

    // GET: api/image/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var image = new ImageDto
        {
            Id = id,
            ImageUrl = "https://example.com/image.jpg",
            IsMain = false
        };

        return Ok(image);
    }

    // POST: api/image
    [HttpPost]
    public IActionResult Create([FromBody] CreateImageDto dto)
    {
        var newId = Guid.NewGuid();
        return CreatedAtAction(nameof(GetById), new { id = newId }, null);
    }

    // PUT: api/image
    [HttpPut]
    public IActionResult Update([FromBody] UpdateImageDto dto)
    {
        // Normalda burada DB-də update ediləcək
        return NoContent();
    }

    // DELETE: api/image/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        // Normalda burada DB-dən silinəcək
        return NoContent();
    }
}
