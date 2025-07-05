using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.DTOs.CategoryDto;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Moderator,Seller")]
public class CategoryController : ControllerBase
{
    private readonly MiniECommerceDbContext _context;

    public CategoryController(MiniECommerceDbContext context)
    {
        _context = context;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories)
            .ToListAsync();

        var categoryDtos = categories.Select(c => MapToDto(c)).ToList();

        return Ok(categoryDtos);
    }

    // GET: api/Category/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound("Kateqoriya tapılmadı.");

        return Ok(MapToDto(category));
    }

    // POST: api/Category
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (dto.ParentCategoryId != null)
        {
            var parentCategory = await _context.Categories.FindAsync(dto.ParentCategoryId);
            if (parentCategory == null)
                return BadRequest("Parent kateqoriya tapılmadı.");
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, MapToDto(category));
    }

    // PUT: api/Category/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound("Kateqoriya tapılmadı.");

        if (dto.ParentCategoryId != null && dto.ParentCategoryId != id)
        {
            var parentCategory = await _context.Categories.FindAsync(dto.ParentCategoryId);
            if (parentCategory == null)
                return BadRequest("Parent kateqoriya tapılmadı.");
        }
        else if (dto.ParentCategoryId == id)
        {
            return BadRequest("Bir kateqoriya özünü parent edə bilməz.");
        }

        category.Name = dto.Name;
        category.ParentCategoryId = dto.ParentCategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound("Kateqoriya tapılmadı.");

        if (category.SubCategories.Any())
            return BadRequest("Alt kateqoriyaları olan kateqoriyanı silmək olmaz.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            SubCategories = category.SubCategories?.Select(MapToDto).ToList() ?? new()
        };
    }
}
