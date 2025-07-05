using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.Favorite;
using MiniECommerce.Domain.Entities;
using System.Security.Claims;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoriteController : ControllerBase
{
    // GET: api/favorite
    [HttpGet]
    public IActionResult GetAll()
    {
        // Bütün favoritləri (və ya istifadəçiyə görə) mock qaytarırıq
        var favorites = new List<FavoriteDto>
        {
            new FavoriteDto
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductTitle = "iPhone 15",
                ProductImage = "iphone15.jpg",
                Price = 2500
            },
            new FavoriteDto
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductTitle = "MacBook Air",
                ProductImage = "macbook.jpg",
                Price = 3200
            }
        };

        return Ok(favorites);
    }

    // GET: api/favorite/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var favorite = new FavoriteDto
        {
            Id = id,
            ProductId = Guid.NewGuid(),
            ProductTitle = "Samsung Galaxy S24",
            ProductImage = "samsung.jpg",
            Price = 1800
        };

        return Ok(favorite);
    }

    // POST: api/favorite
    [HttpPost]
    [Authorize] // Yalnız login olmuş istifadəçi
    public IActionResult Create([FromBody] CreateFavoriteDto dto)
    {
        // İstifadəçi ID-sini JWT-dən al
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Token etibarsız və ya mövcud deyil");

        // Normalda burada Favorite yarat və DB-yə save et
        var newId = Guid.NewGuid();
        return CreatedAtAction(nameof(GetById), new { id = newId }, null);
    }

    // DELETE: api/favorite/{id}
    [HttpDelete("{id}")]
    [Authorize] // Yalnız login olmuş istifadəçi
    public IActionResult Delete(Guid id)
    {
        // Burada userId ilə yoxlaya bilərik ki, həmin favorit bu user-ə aiddir ya yox
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Token etibarsız və ya mövcud deyil");

        // Normalda burada DB-dən silmə əməliyyatı aparılır
        return NoContent();
    }
}
