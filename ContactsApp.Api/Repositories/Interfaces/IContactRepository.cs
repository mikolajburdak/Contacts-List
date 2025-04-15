using ContactsApp.Api.Models;

namespace ContactsApp.Api.Repositories.Interfaces;

public interface IContactRepository
{ 
    Task<IEnumerable<CreateContactList>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(int id);
    Task<Contact> CreateContactAsync(Contact createContact);
    Task UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(int id);
} 