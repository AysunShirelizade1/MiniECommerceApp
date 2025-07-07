using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.ReviewDto;
using MiniECommerce.Domain.Entities;
using System.Security.Claims;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    // GET: api/review
    [HttpGet]
    [AllowAnonymous] // hamıya açıq
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
    [AllowAnonymous]
    public IActionResult GetById(Guid id)
    {
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
    [Authorize(Policy = "Review.Create")]
    public IActionResult Create([FromBody] ReviewCreateDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        // Burada realda DB-yə save etmək lazım
        var newId = Guid.NewGuid();
        return CreatedAtAction(nameof(GetById), new { id = newId }, null);
    }

    // DELETE: api/review/{id}
    [HttpDelete("{id}")]
    [Authorize(Policy = "Review.Delete")]
    public IActionResult Delete(Guid id)
    {
        // Burada silmək əməliyyatını həyata keçir
        return NoContent();
    }
}
