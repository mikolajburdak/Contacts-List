namespace ContactsApp.Api.Models;

public class CreateContactList
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}