using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ContactsApp.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public List<Contact> Contacts { get; set; } = new();
    }
}