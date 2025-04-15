using ContactsApp.Api.DTO;
using Microsoft.AspNetCore.Identity;

namespace ContactsApp.Api.Services.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto model);
    Task<IdentityResult> RegisterAsync(RegisterDto model);
}