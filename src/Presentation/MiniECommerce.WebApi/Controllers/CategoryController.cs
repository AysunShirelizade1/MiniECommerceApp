using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.CategoryDto;

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/category
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories()
    {
        var (items, _) = await _categoryService.GetAllAsync(1, int.MaxValue, "Name", false, null);
        return Ok(items);
    }

    // GET: api/category/filter
    [HttpGet("filter")]
    [Authorize(Policy = "Category.Read")]
    public async Task<IActionResult> GetCategoriesFiltered(
        int pageNumber = 1,
        int pageSize = 10,
        string? sortBy = "Name",
        bool sortDesc = false,
        string? search = null,
        bool flat = false)
    {
        var (items, totalCount) = await _categoryService.GetFilteredAsync(pageNumber, pageSize, sortBy, sortDesc, search, flat);
        return Ok(new
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
        });
    }

    // GET: api/category/{id}/stats
    [HttpGet("{id}/stats")]
    [Authorize(Policy = "Category.Read")]
    public async Task<IActionResult> GetCategoryStats(Guid id)
    {
        var stats = await _categoryService.GetStatsAsync(id);
        if (stats == null)
            return NotFound("Kateqoriya tapılmadı.");

        return Ok(stats);
    }

    // POST: api/category
    [HttpPost]
    [Authorize(Policy = "Category.Create")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newCategoryId = await _categoryService.CreateAsync(dto);

        var createdCategory = await _categoryService.GetByIdAsync(newCategoryId);

        return CreatedAtAction(nameof(GetAllCategories), new { id = newCategoryId }, createdCategory);
    }

    // PUT: api/category/{id}
    [HttpPut("{id}")]
    [Authorize(Policy = "Category.Update")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _categoryService.UpdateAsync(id, dto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Kateqoriya tapılmadı.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    // DELETE: api/category/{id} (Soft Delete)
    [HttpDelete("{id}")]
    [Authorize(Policy = "Category.Delete")]
    public async Task<IActionResult> SoftDeleteCategory(Guid id)
    {
        try
        {
            await _categoryService.SoftDeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Kateqoriya tapılmadı.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    // POST: api/category/bulk-delete
    [HttpPost("bulk-delete")]
    [Authorize(Policy = "Category.Delete")]
    public async Task<IActionResult> BulkDelete([FromBody] List<Guid> ids)
    {
        var result = await _categoryService.BulkSoftDeleteAsync(ids);
        return Ok(new { Success = result });
    }

    // GET: api/category/tree
    [HttpGet("tree")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryTree()
    {
        var tree = await _categoryService.GetCategoryTreeAsync();
        return Ok(tree);
    }
}
