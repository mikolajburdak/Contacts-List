using ContactsApp.Api.DTO;
using ContactsApp.Api.Mappers;
using ContactsApp.Api.Models;
using ContactsApp.Api.Repositories.Interfaces;
using ContactsApp.Api.Services.Interfaces;

namespace ContactsApp.Api.Services.Impl;

public class ContactServiceImpl : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactServiceImpl> _logger;

    public ContactServiceImpl(IContactRepository contactRepository, ILogger<ContactServiceImpl> logger)
    {
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<IEnumerable<ContactListDto>> GetAllContactsAsync()
    {
        var contacts = await _contactRepository.GetAllContactsAsync();
        return ContactMapper.ToDtoList(contacts);
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _contactRepository.GetContactByIdAsync(id);
    }

    public async Task<Contact> CreateContactAsync(CreateContactDto contactDto, string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidOperationException("User ID is required to create a contact.");
        }

        var contact = ContactMapper.ToContact(contactDto, userId);
        contact.CreatedAt = DateTime.UtcNow;
        
        return await _contactRepository.CreateContactAsync(contact);
    }

    public async Task<bool> PartialUpdateContactAsync(int contactId, ContactUpdateRequestDto contactUpdateRequestDto, string userId)
    {
        var contact = await _contactRepository.GetContactByIdAsync(contactId);
        if (contact is null || contact.UserId != userId)
            return false;

        ContactMapper.MapUpdate(contact, contactUpdateRequestDto);
        await _contactRepository.UpdateContactAsync(contact);
        return true;
    }

    public async Task DeleteContactAsync(int id)
    {
        await _contactRepository.DeleteContactAsync(id);
    }
}