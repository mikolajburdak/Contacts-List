using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactsApp.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; 

        [JsonIgnore]
        public List<Subcategory> Subcategories { get; set; } = new();
        
        [JsonIgnore]
        public List<Contact> Contacts { get; set; } = new();
    }
}