using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // np. Służbowy, Prywatny, Inny

        public List<Subcategory> Subcategories { get; set; } = new();
        public List<Contact> Contacts { get; set; } = new();
    }
}