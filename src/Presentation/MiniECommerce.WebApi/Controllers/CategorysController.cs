using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.CategoryDto;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET /api/categories
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllNestedAsync();
        return Ok(categories);
    }

    // POST /api/categories
    // Yalnız Admin rolundakı istifadəçilər əlavə edə bilər
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), null);
    }
}
