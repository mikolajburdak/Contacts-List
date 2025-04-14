using ContactsApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Przykład unikalności emaila
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Relacje / seed data itp. możesz dodać tutaj
        }*/
    }
}