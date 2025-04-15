using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Api.Models
{
    public class Contact
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
        
        // Pole nie jest mapowane do bazy danych
        [NotMapped]
        public DateTime? CreatedAt { get; set; }

        [Required]
        public int CategoryId { get; set; }
        
        [JsonIgnore]
        public Category Category { get; set; }

        [Required]
        public int? SubcategoryId { get; set; }
        
        [JsonIgnore]
        public Subcategory Subcategory { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;
    }
}