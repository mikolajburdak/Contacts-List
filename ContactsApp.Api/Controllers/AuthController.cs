using ContactsApp.Api.DTO;
using ContactsApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await _authService.RegisterAsync(model);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var token = await _authService.LoginAsync(model);
        if (token == null)
        {
            return Unauthorized();
        }
        
        return Ok(new { token });
    }
}