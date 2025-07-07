using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Email;

namespace MiniECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "SendTestEmail")] // yalnız icazəsi olan rol və ya istifadəçilər
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }


    /// <param name="dto">Email göndəriş detalları</param>
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.To) || string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Body))
            return BadRequest("To, Subject və Body boş ola bilməz.");

        try
        {
            await _emailService.SendAsync(dto);
            return Ok("Email uğurla göndərildi.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Email göndərilərkən xəta baş verdi: {ex.Message}");
        }
    }
}
