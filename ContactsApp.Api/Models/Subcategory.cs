using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Api.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // np. Szef, Klient, itp.

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Contact> Contacts { get; set; } = new();
    }
}