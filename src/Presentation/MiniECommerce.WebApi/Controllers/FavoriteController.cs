using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Favorite;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]  
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoriteController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    // GET: api/favorite
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var favorites = await _favoriteService.GetAllByUserIdAsync(userId);
        return Ok(favorites);
    }

    // GET: api/favorite/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var favorite = await _favoriteService.GetByIdAsync(id);

        if (favorite == null)
            return NotFound("Favorit tapılmadı.");

        if (favorite.Id != userId)
            return Forbid("Bu favoriti görmək üçün icazəniz yoxdur.");

        return Ok(favorite);
    }


    // POST: api/favorite
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFavoriteDto dto)
    {
        var userId = GetUserId();
        var newId = await _favoriteService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = newId }, null);
    }

    // DELETE: api/favorite/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        try
        {
            await _favoriteService.DeleteAsync(id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("Bu favoriti silmək üçün icazəniz yoxdur.");
        }
        catch (Exception)
        {
            return NotFound("Favorit tapılmadı.");
        }
    }

    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userIdString!);
    }
}
