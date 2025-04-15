using ContactsApp.Api.DTO;
using ContactsApp.Api.Models;

namespace ContactsApp.Api.Services.Interfaces;

public interface IContactService
{
    Task<IEnumerable<ContactListDto>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(int id);
    Task<Contact> CreateContactAsync(CreateContactDto contactDto, string userId);
    Task<bool> PartialUpdateContactAsync(int id, ContactUpdateRequestDto contactUpdateRequestDto, string userId);
    Task DeleteContactAsync(int id);
}