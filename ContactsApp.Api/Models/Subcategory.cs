using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactsApp.Api.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; 

        [Required]
        public int CategoryId { get; set; }
        
        [JsonIgnore]
        public Category Category { get; set; }

        [JsonIgnore]
        public List<Contact> Contacts { get; set; } = new();
    }
}