using ContactsApp.Api.DTO;
using ContactsApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    
    [Authorize]
    [HttpGet("test")]
    public IActionResult TestAuth()
    {
        // Wyświetlenie wszystkich claimów - pomocne w diagnostyce
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        return Ok(new 
        { 
            message = "Authorized successfully!", 
            userId,
            claims
        });
    }
}