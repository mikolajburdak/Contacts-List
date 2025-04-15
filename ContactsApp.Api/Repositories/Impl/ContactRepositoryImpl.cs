using ContactsApp.Api.Data;
using ContactsApp.Api.Models;
using ContactsApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Api.Repositories.Impl;

public class ContactRepositoryImpl : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepositoryImpl(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CreateContactList>> GetAllContactsAsync()
    {
        var contacts = await _context.Contacts
            .Select(c => new CreateContactList
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName
            })
            .ToListAsync();

        return contacts;
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        try
        {
            var contact = await _context.Contacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
                return null;

            try
            {
                contact.Category = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == contact.CategoryId);
                
                contact.Subcategory = await _context.Subcategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == contact.SubcategoryId);
            }
            catch
            {
            }

            return contact;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Contact> CreateContactAsync(Contact contact)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == contact.CategoryId);
        if (!categoryExists)
        {
            throw new InvalidOperationException($"Category with ID {contact.CategoryId} does not exist.");
        }

        if (contact.SubcategoryId.HasValue)
        {
            var subcategoryExists = await _context.Subcategories.AnyAsync(c => c.Id == contact.SubcategoryId);
            if (!subcategoryExists)
            {
                throw new InvalidOperationException($"Subcategory with ID {contact.SubcategoryId} does not exist.");
            }
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == contact.UserId);
        if (!userExists)
        {
            throw new InvalidOperationException($"User with ID {contact.UserId} does not exist.");
        }

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(int id)
    {
        var contact = await GetContactByIdAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}