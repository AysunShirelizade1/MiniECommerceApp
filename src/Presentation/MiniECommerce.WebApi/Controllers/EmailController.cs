using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Email;

namespace MiniECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "SendTestEmail")] // YALNIZ bu policy olanlar icazə alacaq
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-test")]
    public async Task<IActionResult> SendTestEmail()
    {
        var emailDto = new EmailDto
        {
            To = "sunahacker01@gmail.com",
            Subject = "Test Email from MiniECommerce",
            Body = "<strong>Bu email SMTP ilə göndərildi!</strong>"
        };

        try
        {
            await _emailService.SendAsync(emailDto);
            return Ok("Email uğurla göndərildi!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Email göndərilərkən xəta baş verdi: {ex.Message}");
        }
    }
}
