namespace ContactsApp.Api.DTO;

public class ContactUpdateRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? CategoryId { get; set; }
    public int? SubcategoryId { get; set; }
}