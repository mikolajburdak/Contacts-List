using ContactsApp.Api.Data;
using ContactsApp.Api.Models;
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
        return await _context.Contacts
            .Include(c => c.Category)       // jeśli chcesz ładować relacje
            .Include(c => c.Subcategory)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateContactAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
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