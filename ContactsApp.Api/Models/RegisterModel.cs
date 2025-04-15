using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Api.Models;

public class RegisterModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;
    
    [Required] 
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
}