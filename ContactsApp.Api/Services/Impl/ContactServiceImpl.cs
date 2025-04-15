using ContactsApp.Api.DTO;
using ContactsApp.Api.Mappers;
using ContactsApp.Api.Models;
using ContactsApp.Api.Repositories;
using ContactsApp.Api.Services.Interfaces;

namespace ContactsApp.Api.Services.Impl;

public class ContactServiceImpl : IContactService
{
    
    private readonly IContactRepository _contactRepository;
    
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
        var contact = ContactMapper.ToContact(contactDto, userId);
        await _contactRepository.CreateContactAsync(contact);
        return contact;
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