using Microsoft.AspNetCore.Mvc;
using MiniECommerce.Application.DTOs.AppUserDto;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var success = await _authService.RegisterAsync(registerDto);
        if (!success)
            return BadRequest("User already exists or registration failed.");

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var tokenDto = await _authService.LoginAsync(loginDto);
        if (tokenDto == null)
            return Unauthorized("Invalid email or password.");

        return Ok(tokenDto);
    }
}
