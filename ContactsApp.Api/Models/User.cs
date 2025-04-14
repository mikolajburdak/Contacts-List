using System.ComponentModel.DataAnnotations;
namespace ContactsApp.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public List<Contact> Contacts { get; set; } = new();
    }
}