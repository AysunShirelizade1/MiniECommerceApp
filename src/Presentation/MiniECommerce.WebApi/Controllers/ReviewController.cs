using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.ReviewDto;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    // GET: api/review
    [HttpGet]
    public IActionResult GetAll()
    {
        var mockReviews = new List<ReviewDto>
        {
            new ReviewDto(new Review
            {
                Id = Guid.NewGuid(),
                Comment = "Məhsul çox keyfiyyətlidir",
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            }),
            new ReviewDto(new Review
            {
                Id = Guid.NewGuid(),
                Comment = "Çatdırılma gecikdi, amma məhsul yaxşıdır",
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            })
        };

        return Ok(mockReviews);
    }

    // GET: api/review/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        // Mock Review
        var review = new Review
        {
            Id = id,
            Comment = "Test şərhi",
            ProductId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        return Ok(new ReviewDto(review));
    }

    // POST: api/review
    [HttpPost]
    public IActionResult Create([FromBody] ReviewCreateDto dto)
    {
        // Normalda: yeni review yaradıb DB-yə save edərsən
        var newId = Guid.NewGuid();
        return CreatedAtAction(nameof(GetById), new { id = newId }, null);
    }

    // DELETE: api/review/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        // Normalda: review silinər
        return NoContent();
    }
}
