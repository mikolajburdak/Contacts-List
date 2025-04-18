using System.ComponentModel.DataAnnotations;
using ContactsApp.Api.Models;

namespace ContactsApp.Api.DTO;

public class ContactDto
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; }

    public DateTime BirthDate { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    [Required]
    public int SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; }
    
}
